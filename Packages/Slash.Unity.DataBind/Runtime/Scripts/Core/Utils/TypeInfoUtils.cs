using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Slash.Unity.DataBind.Core.Utils
{
    /// <summary>
    ///   Utility methods for <see cref="System.Type"/>. 
    ///   Collected in this class to not spread NETFX_CORE #ifs all over the code.
    /// </summary>
    public static class TypeInfoUtils
    {
#if !NETFX_CORE
        /// <summary>
        ///     Creates a delegate of the specified type that represents the specified static or instance method, with the
        ///     specified
        ///     first argument.
        /// </summary>
        /// <param name="type">The Type of delegate to create.</param>
        /// <param name="target">The object to which the delegate is bound, or null to treat method as static. </param>
        /// <param name="method">The MethodInfo describing the static or instance method the delegate is to represent.</param>
        /// <returns>A delegate of the specified type that represents the specified static or instance method. </returns>
        public static Delegate CreateDelegate(Type type, object target, MethodInfo method)
        {
            return Delegate.CreateDelegate(type, target, method);
        }

        /// <summary>
        ///     Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <param name="baseType">Base type to get the types of.</param>
        /// <returns>List of found types.</returns>
        public static List<Type> FindTypesWithBase(Type baseType)
        {
            var types = new List<Type>();
            foreach (var assembly in AssemblyUtils.GetLoadedAssemblies())
                try
                {
                    types.AddRange(assembly.GetTypes().Where(baseType.IsAssignableFrom));
                }
                catch (ReflectionTypeLoadException)
                {
                    // Some assemblies might not be accessible, skip them.
                }

            return types;
        }

        /// <summary>
        ///     Returns the base type of the specified type.
        /// </summary>
        /// <param name="type">Type to get base type for.</param>
        /// <returns>Base type of specified type.</returns>
        public static Type GetBaseType(Type type)
        {
            return type.BaseType;
        }

        /// <summary>
        ///     Searches for the public field with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the public field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetPublicField(Type type, string name)
        {
            return type.GetField(name, BindingFlags.Instance | BindingFlags.Public);
        }

        /// <summary>
        ///     Searches for the private field with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <param name="additionalFlags">Additional flags to search for private field.</param>
        /// <returns>An object representing the private field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetPrivateField(Type type, string name, BindingFlags additionalFlags = BindingFlags.Default)
        {
            return type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic | additionalFlags);
        }

        /// <summary>
        ///     Searches for the public method with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the public method to get. </param>
        /// <returns>An object that represents the public method with the specified name, if found; otherwise, null.</returns>
        public static MethodInfo GetPublicMethod(Type type, string name)
        {
            return type.GetMethod(name);
        }

        /// <summary>
        ///     Searches for the public property with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the public property to get. </param>
        /// <param name="additionalFlags">Additional flags to search for private field.</param>
        /// <returns>An object that represents the public property with the specified name, if found; otherwise, null.</returns>
        public static PropertyInfo GetPublicProperty(Type type, string name, BindingFlags additionalFlags = BindingFlags.Default)
        {
            return type.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | additionalFlags);
        }

        /// <summary>
        ///     Indicates if the specified type is an enum type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if the specified type is an enum type; otherwise, false.</returns>
        public static bool IsEnum(Type type)
        {
            return type.IsEnum;
        }

        /// <summary>
        ///   Indicates if type is a value type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if type is a value type; otherwise, false.</returns>
        public static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }

        /// <summary>
        ///   Indicates if type is a generic type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if type is a generic type; otherwise, false.</returns>
        public static bool IsGenericType(Type type)
        {
            return type.IsGenericType;
        }

        /// <summary>
        ///   Returns the generic arguments of the type.
        /// </summary>
        /// <param name="type">Type to get generic arguments for.</param>
        /// <returns>Array of generic arguments.</returns>
        public static Type[] GetGenericArguments(Type type)
        {
            return type.GetGenericArguments();
        }

        /// <summary>
        ///   Returns the implemented interfaces by the type.
        /// </summary>
        /// <param name="type">Type to get implemented interfaces for.</param>
        /// <returns>Enumeration of implemented interfaces.</returns>
        public static IEnumerable<Type> GetInterfaces(Type type)
        {
            return type.GetInterfaces();
        }

        /// <summary>
        ///   Returns the method info for the specified delegate.
        /// </summary>
        /// <param name="del">Delegate to get method info for.</param>
        /// <returns>Method info for the specified delegate.</returns>
        public static MethodInfo GetMethodInfo(Delegate del)
        {
            return del.Method;
        }

#else
        /// <summary>
        ///   Creates a delegate of the specified type that represents the specified static or instance method, with the specified
        ///   first argument.
        /// </summary>
        /// <param name="type">The Type of delegate to create.</param>
        /// <param name="target">The object to which the delegate is bound, or null to treat method as static. </param>
        /// <param name="method">The MethodInfo describing the static or instance method the delegate is to represent.</param>
        /// <returns>A delegate of the specified type that represents the specified static or instance method. </returns>
        public static Delegate CreateDelegate(Type type, object target, MethodInfo method)
        {
            return method.CreateDelegate(type, target);
        }

        /// <summary>
        ///   Searches all loaded assemblies and returns the types which have the specified attribute.
        /// </summary>
        /// <param name="baseType">Base type to get the types of.</param>
        /// <returns>List of found types.</returns>
        public static List<Type> FindTypesWithBase(Type baseType)
        {
            var types = new List<Type>();
            foreach (var assembly in AssemblyUtils.GetLoadedAssemblies())
            {
                types.AddRange(
                    assembly.DefinedTypes.Where(baseType.GetTypeInfo().IsAssignableFrom)
                        .Select(typeInfo => typeInfo.AsType()));
            }

            return types;
        }

        /// <summary>
        ///   Returns the base type of the specified type.
        /// </summary>
        /// <param name="type">Type to get base type for.</param>
        /// <returns>Base type of specified type.</returns>
        public static Type GetBaseType(Type type)
        {
            return type.GetTypeInfo().BaseType;
        }

        /// <summary>
        ///   Searches for the public field with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the public field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetPublicField(Type type, string name)
        {
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredField(name))
                    .FirstOrDefault(field => field != null && field.IsPublic);
        }

        /// <summary>
        ///   Searches for the private field with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the private field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetPrivateField(Type type, string name)
        {
            // Issue #51: IsPrivate flag always false.
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredField(name))
                    .FirstOrDefault(field => field != null && !field.IsPublic);
        }

        /// <summary>
        ///   Searches for the public method with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the public method to get. </param>
        /// <returns>An object that represents the public method with the specified name, if found; otherwise, null.</returns>
        public static MethodInfo GetPublicMethod(Type type, string name)
        {
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredMethod(name))
                    .FirstOrDefault(method => method != null && method.IsPublic);
        }

        /// <summary>
        ///   Searches for the public property with the specified name.
        /// </summary>
        /// <param name="type">Type to search in.</param>
        /// <param name="name">The string containing the name of the public property to get. </param>
        /// <returns>An object that represents the public property with the specified name, if found; otherwise, null.</returns>
        public static PropertyInfo GetPublicProperty(Type type, string name)
        {
            // https://msdn.microsoft.com/en-us/library/kz0a8sxy(v=vs.110).aspx
            // A property is considered public to reflection if it has at least one accessor that is public.
            return
                GetBaseTypes(type)
                    .Select(baseType => baseType.GetTypeInfo().GetDeclaredProperty(name))
                    .FirstOrDefault(property => property != null && (property.GetMethod.IsPublic || property.SetMethod.IsPublic));
        }

        private static IEnumerable<Type> GetBaseTypes(Type type)
        {
            yield return type;

            var baseType = type.GetTypeInfo().BaseType;

            if (baseType != null)
            {
                foreach (var t in GetBaseTypes(baseType))
                {
                    yield return t;
                }
            }
        }

        /// <summary>
        ///   Indicates if the specified type is an enum type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if the specified type is an enum type; otherwise, false.</returns>
        public static bool IsEnum(Type type)
        {
            return type.GetTypeInfo().IsEnum;
        }
        
        /// <summary>
        ///   Indicates if type is a value type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if type is a value type; otherwise, false.</returns>
        public static bool IsValueType(Type type)
        {
            return type.GetTypeInfo().IsValueType;
        }
        
        /// <summary>
        ///   Indicates if type is a generic type.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>True if type is a generic type; otherwise, false.</returns>
        public static bool IsGenericType(Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }
        
        /// <summary>
        ///   Returns the generic arguments of the type.
        /// </summary>
        /// <param name="type">Type to get generic arguments for.</param>
        /// <returns>Array of generic arguments.</returns>
        public static Type[] GetGenericArguments(Type type)
        {
            return type.GetTypeInfo().GenericTypeArguments;
        }
        
        /// <summary>
        ///   Returns the implemented interfaces by the type.
        /// </summary>
        /// <param name="type">Type to get implemented interfaces for.</param>
        /// <returns>Enumeration of implemented interfaces.</returns>
        public static IEnumerable<Type> GetInterfaces(Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces;
        }
        
        /// <summary>
        ///   Returns the method info for the specified delegate.
        /// </summary>
        /// <param name="del">Delegate to get method info for.</param>
        /// <returns>Method info for the specified delegate.</returns>
        public static MethodInfo GetMethodInfo(Delegate del)
        {
            return del.GetMethodInfo();
        }
#endif
    }
}