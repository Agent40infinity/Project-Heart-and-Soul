namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;

    /// <summary>
    ///   Attribute that provides an explicit type hint for the inspector.
    /// </summary>
    public class DataTypeHintExplicitAttribute : DataTypeHintAttribute
    {
        private readonly Type type;

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="type">Explicit type to provide as a hint for the inspector.</param>
        public DataTypeHintExplicitAttribute(Type type)
        {
            this.type = type;
        }

        /// <inheritdoc />
        public override Type GetTypeHint(Type classType)
        {
            return this.type;
        }
    }
}