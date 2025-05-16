namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;

    /// <summary>
    ///   Attribute that provides the generic type of the property/field it is set on as the type hint for the inspector.
    /// </summary>
    public class DataTypeHintGenericTypeAttribute : DataTypeHintAttribute
    {
        private readonly int genericTypeIndex;

        /// <inheritdoc />
        public DataTypeHintGenericTypeAttribute(int genericTypeIndex = 0)
        {
            this.genericTypeIndex = genericTypeIndex;
        }

        /// <inheritdoc />
        public override Type GetTypeHint(Type classType)
        {
            // Return generic type.
            var genericArguments = classType.GetGenericArguments();
            return this.genericTypeIndex < genericArguments.Length ? genericArguments[this.genericTypeIndex] : null;
        }
    }
}