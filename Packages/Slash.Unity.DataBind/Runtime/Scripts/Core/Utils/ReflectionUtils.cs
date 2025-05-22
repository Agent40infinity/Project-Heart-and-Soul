// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReflectionUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    ///     Provides utility methods for reflecting types and members.
    /// </summary>
    public static class ReflectionUtils
    {
        /// <summary>
        ///     <para>
        ///         Looks up the specified full type name in all loaded assemblies,
        ///         ignoring assembly version.
        ///     </para>
        ///     <para>
        ///         In order to understand how to access generic types,
        ///         see http://msdn.microsoft.com/en-us/library/w3f99sx1.aspx.
        ///     </para>
        /// </summary>
        /// <param name="fullName">Full name of the type to find.</param>
        /// <returns>Type with the specified name.</returns>
        /// <exception cref="TypeLoadException">If the type couldn't be found.</exception>
        public static Type FindType(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                return null;
            }

            // Split type name from .dll version.
            fullName = SystemExtensions.RemoveAssemblyInfo(fullName);

            var type = Type.GetType(fullName);

            if (type != null)
            {
                return type;
            }

            foreach (var asm in AssemblyUtils.GetLoadedAssemblies())
            {
                type = asm.GetType(fullName);
                if (type != null)
                {
                    return type;
                }
            }

            throw new TypeLoadException(string.Format("Unable to find type {0}.", fullName));
        }

        /// <summary>
        ///     Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <returns>List of found types.</returns>
        /// <typeparam name="T">Type of the attribute to get the types of.</typeparam>
        public static IEnumerable<Type> FindTypesWithBase<T>() where T : class
        {
            return TypeInfoUtils.FindTypesWithBase(typeof(T));
        }

        /// <summary>
        ///     Returns the default value for the specified type.
        /// </summary>
        /// <param name="type">Type to get default value for.</param>
        /// <returns>Default value for the specified type.</returns>
        public static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        /// <summary>
        ///     Returns the item type of an enumerable type.
        /// </summary>
        /// <param name="enumerableType">Type of enumerable to get item type for.</param>
        /// <returns>Type of items in the specified enumerable type.</returns>
        public static Type GetEnumerableItemType(Type enumerableType)
        {
            if (!enumerableType.IsGenericType)
            {
                throw new ArgumentException("enumerable",
                    "Type must be IEnumerable<>, but was " + enumerableType.FullName);
            }

            if (!IsAssignableToGenericType(enumerableType, typeof(IEnumerable<>)))
            {
                throw new ArgumentException("enumerable",
                    "Type must be IEnumerable<>, but was " + enumerableType.FullName);
            }

            return enumerableType.GetGenericArguments()[0];
        }

        /// <summary>
        ///     Returns the type info for the member with the specified name of the specified type.
        /// </summary>
        /// <param name="type">Type to get member info from.</param>
        /// <param name="name">Name of member to get info for.</param>
        /// <returns>Type info for the member with the specified name of the specified type.</returns>
        public static NodeTypeInfo GetNodeTypeInfo(Type type, string name)
        {
            // Get item if collection.
            var typeInterfaces = TypeInfoUtils.GetInterfaces(type);
            if (typeInterfaces.Contains(typeof(IEnumerable)))
            {
                // Check if index provided.
                int itemIndex;
                if (int.TryParse(name, out itemIndex))
                {
                    // Get item type.
                    var itemType = type.GetElementType();
                    if (itemType == null)
                    {
                        if (TypeInfoUtils.IsGenericType(type))
                        {
                            var genericArguments = TypeInfoUtils.GetGenericArguments(type);
                            itemType = genericArguments.Length > 0 ? genericArguments[0] : typeof(object);
                        }
                        else
                        {
                            itemType = typeof(object);
                        }
                    }

                    // Return item.
                    return new EnumerableNode {Type = itemType, Index = itemIndex};
                }
            }

            // Get property.
            var reflectionProperty = TypeInfoUtils.GetPublicProperty(type, name);
            if (reflectionProperty != null)
            {
                return new PropertyNode {Type = reflectionProperty.PropertyType, Property = reflectionProperty};
            }

            // Get field.
            var reflectionField = TypeInfoUtils.GetPublicField(type, name);
            if (reflectionField != null)
            {
                return new FieldNode {Type = reflectionField.FieldType, Field = reflectionField};
            }

            // Get method.
            var reflectionMethod = TypeInfoUtils.GetPublicMethod(type, name);
            if (reflectionMethod != null)
            {
                return new MethodNode {Type = reflectionMethod.ReturnType, Method = reflectionMethod};
            }

            return null;
        }

        /// <summary>
        ///     Tests if a given type is assignable to a generic type.
        /// </summary>
        /// <param name="givenType">Type to check if assignable.</param>
        /// <param name="genericType">Generic type to be assignable to.</param>
        /// <returns>True if given type is assignable to generic type; otherwise, false.</returns>
        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            var baseType = givenType.BaseType;
            if (baseType == null)
            {
                return false;
            }

            return IsAssignableToGenericType(baseType, genericType);
        }

        /// <summary>
        ///     Tries to convert the specified value to the specified value.
        /// </summary>
        /// <param name="rawValue">Value to convert.</param>
        /// <param name="convertedValue">Converted value.</param>
        /// <returns>True if value could be converted; otherwise, false.</returns>
        public static bool TryConvertValue<T>(object rawValue, out T convertedValue)
        {
            object convertedValueObject;
            if (TryConvertValue(rawValue, typeof(T), out convertedValueObject))
            {
                convertedValue = (T) convertedValueObject;
                return true;
            }
            convertedValue = default(T);
            return false;
        }

        /// <summary>
        ///     Tries to convert the specified value to the specified value.
        /// </summary>
        /// <param name="rawValue">Value to convert.</param>
        /// <param name="type">Type to convert to.</param>
        /// <param name="convertedValue">Converted value.</param>
        /// <returns>True if value could be converted; otherwise, false.</returns>
        public static bool TryConvertValue(object rawValue, Type type, out object convertedValue)
        {
            try
            {
                // Try convert enum.
                if (TypeInfoUtils.IsEnum(type))
                {
                    if (!Enum.IsDefined(type, rawValue))
                    {
                        convertedValue = null;
                        return false;
                    }

                    var stringValue = rawValue as string;
                    convertedValue = stringValue != null ? Enum.Parse(type, stringValue) : Enum.ToObject(type, rawValue);
                    return true;
                }
                convertedValue = Convert.ChangeType(rawValue, type);
                return true;
            }
            catch (Exception)
            {
                convertedValue = null;
                return false;
            }
        }
    }

    /// <summary>
    ///     Type info for a field member.
    /// </summary>
    public class FieldNode : NodeTypeInfo
    {
        /// <inheritdoc />
        public override bool CanValueChange
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///     Field info.
        /// </summary>
        public FieldInfo Field { private get; set; }

        /// <inheritdoc />
        public override object GetValue(object parentObject)
        {
            if (parentObject == null)
            {
                return ReflectionUtils.GetDefaultValue(this.Type);
            }

            // Get field value.
            if (this.Field != null)
            {
                return this.Field.GetValue(parentObject);
            }

            return null;
        }

        /// <inheritdoc />
        public override void SetValue(object parentObject, object value)
        {
            if (parentObject == null)
            {
                return;
            }

            if (this.Field == null)
            {
                return;
            }

            // Set field value.
            this.Field.SetValue(parentObject, value);
        }
    }

	/// <summary>
	///     Type info for a LocalizedString field member.
	/// </summary>
	public class LocFieldNode : NodeTypeInfo
	{
		/// <inheritdoc />
		public override bool CanValueChange
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		///     Field info.
		/// </summary>
		public FieldInfo Field { private get; set; }

		/// <inheritdoc />
		public override object GetValue(object parentObject)
		{
			if (parentObject == null)
			{
				return ReflectionUtils.GetDefaultValue(this.Type);
			}

			return null;
		}

		/// <inheritdoc />
		public override void SetValue(object parentObject, object value)
		{
			if (parentObject == null)
			{
				return;
			}

			if (this.Field == null)
			{
				return;
			}

			// Set field value.
			this.Field.SetValue(parentObject, value);
		}
	}

	/// <summary>
	///     Type info for a property member.
	/// </summary>
	public class PropertyNode : NodeTypeInfo
    {
        /// <inheritdoc />
        public override bool CanValueChange
        {
            get
            {
                // Even properties which can't be written may change their value internally.
                return this.Property != null;
            }
        }

        /// <summary>
        ///     Property info.
        /// </summary>
        public PropertyInfo Property { private get; set; }

        /// <inheritdoc />
        public override object GetValue(object parentObject)
        {
            if (parentObject == null)
            {
                return ReflectionUtils.GetDefaultValue(this.Type);
            }

            // Get property value.
            if (this.Property != null)
            {
                return this.Property.GetValue(parentObject, null);
            }

            return null;
        }

        /// <inheritdoc />
        public override void SetValue(object parentObject, object value)
        {
            if (parentObject == null)
            {
                return;
            }

            if (this.Property == null)
            {
                return;
            }

            // Set property value.
            if (this.Property.CanWrite)
            {
                this.Property.SetValue(parentObject, value, null);
            }
            else
            {
                throw new InvalidOperationException("Property '" + this.Property.Name + "' is read-only.");
            }
        }
    }

    /// <summary>
    ///     Type info for a method member.
    /// </summary>
    public class MethodNode : NodeTypeInfo
    {
        /// <inheritdoc />
        public override bool CanValueChange
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        ///     Method info.
        /// </summary>
        public MethodInfo Method { private get; set; }

        /// <inheritdoc />
        public override object GetValue(object parentObject)
        {
            if (parentObject == null)
            {
                return null;
            }

            // Get delegate.
            if (this.Method != null)
            {
                var args = new List<Type>(this.Method.GetParameters().Select(p => p.ParameterType));
                var delegateType = Expression.GetActionType(args.ToArray());
                return TypeInfoUtils.CreateDelegate(delegateType, parentObject, this.Method);
            }

            return null;
        }
    }

    /// <summary>
    ///     Base class for member type info.
    /// </summary>
    public abstract class NodeTypeInfo
    {
        /// <summary>
        ///     Indicates if the node can have children.
        /// </summary>
        public bool CanHaveChildren
        {
            get
            {
                return !this.Type.IsPrimitive && !this.Type.IsEnum;
            }
        }

        /// <summary>
        ///     Indicates if this the value of this node can change.
        /// </summary>
        public abstract bool CanValueChange { get; }

        /// <summary>
        ///     Type of member.
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        ///     Returns the value using the type info on the specified object.
        /// </summary>
        /// <param name="parentObject">Object to use the type info on.</param>
        /// <returns>Current value when using type info on specified object.</returns>
        public abstract object GetValue(object parentObject);

        /// <summary>
        ///     Set the value of the member in the specified object to the specified value.
        /// </summary>
        /// <param name="parentObject">Object to set value in.</param>
        /// <param name="value">Value to set.</param>
        public virtual void SetValue(object parentObject, object value)
        {
            throw new InvalidOperationException("Data node of type '" + this.Type + "' is read-only.");
        }
    }

    /// <summary>
    ///     Type info for an object.
    /// </summary>
    public class ObjectNodeTypeInfo : NodeTypeInfo
    {
        private object obj;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="obj">Object the node type info is for.</param>
        public ObjectNodeTypeInfo(object obj)
        {
            this.obj = obj;
            this.Type = obj.GetType();
        }

        /// <inheritdoc />
        public override bool CanValueChange
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public override object GetValue(object parentObject)
        {
            return this.obj;
        }

        /// <inheritdoc />
        public override void SetValue(object parentObject, object value)
        {
            this.obj = value;
        }
    }

    /// <summary>
    ///     Type info for an enumerable member.
    /// </summary>
    public class EnumerableNode : NodeTypeInfo
    {
        /// <inheritdoc />
        public override bool CanValueChange
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///     Index of item to access in enumerable.
        /// </summary>
        public int Index { get; set; }

        /// <inheritdoc />
        public override object GetValue(object parentObject)
        {
            // Check if enumerable.
            var enumerable = parentObject as IEnumerable;
            if (enumerable == null)
            {
                return ReflectionUtils.GetDefaultValue(this.Type);
            }

            var index = 0;
            foreach (var item in enumerable)
            {
                if (index == this.Index)
                {
                    return item;
                }
                ++index;
            }

            return null;
        }
    }
}