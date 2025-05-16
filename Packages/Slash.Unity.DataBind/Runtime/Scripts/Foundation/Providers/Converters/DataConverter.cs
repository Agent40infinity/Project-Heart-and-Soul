// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using Slash.Unity.DataBind.Core.Presentation;
    
    /// <summary>
    ///   Converter which converts its bound value.
    /// </summary>
    public abstract class DataConverter : DataProvider
    {
        /// <summary>
        ///   Data value to use.
        /// </summary>
        public DataBinding Data;

        /// <summary>
        ///   Converts the specified value.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value.</returns>
        public abstract object Convert(object value);

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();

            this.RemoveBinding(this.Data);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();

            this.AddBinding(this.Data);
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }

    /// <summary>
    ///   Converter which converts its provided value to a target type.
    /// </summary>
    /// <typeparam name="TFrom">Expected type of data value.</typeparam>
    /// <typeparam name="TTo">Type to convert data value to.</typeparam>
    public abstract class DataConverter<TFrom, TTo> : DataConverter
    {
        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                var value = this.Data.GetValue<TFrom>();
                return this.Convert(value);
            }
        }

        /// <inheritdoc />
        public override object Convert(object value)
        {
            return this.Convert((TFrom)value);
        }

        /// <summary>
        ///   Called when the specified value should be converted.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value.</returns>
        protected abstract TTo Convert(TFrom value);
    }
}