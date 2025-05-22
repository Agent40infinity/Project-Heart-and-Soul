namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    /// <summary>
    ///     Base generic class for a value converter.
    /// </summary>
    public abstract class ValueConverter<TInput, TOutput> : ValueConverter
    {
        /// <inheritdoc />
        public override object Convert(object value)
        {
            return this.Convert((TInput) value);
        }

        /// <summary>
        ///     Called when the specified value should be converted.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value.</returns>
        protected abstract TOutput Convert(TInput value);
    }

    /// <summary>
    ///     Base class for a value converter.
    /// </summary>
    public abstract class ValueConverter
    {
        /// <summary>
        ///     Converts the specified value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value.</returns>
        public abstract object Convert(object value);
    }
}