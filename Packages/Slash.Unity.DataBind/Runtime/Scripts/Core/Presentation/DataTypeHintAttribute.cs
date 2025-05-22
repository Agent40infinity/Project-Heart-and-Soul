namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;

    /// <summary>
    ///   Attribute that provides a type hint for the inspector.
    /// </summary>
    public abstract class DataTypeHintAttribute : Attribute
    {
        /// <summary>
        ///     Returns the type hint.
        /// </summary>
        /// <param name="classType">Type of class the attribute was found in.</param>
        /// <returns>Type hint.</returns>
        public abstract Type GetTypeHint(Type classType);
    }
}