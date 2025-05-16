namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using System.Globalization;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Creates a currency string from the provided number and culture name.
    ///     <para>Input: Number</para>
    ///     <para>Input: ISO culture name</para>
    ///     <para>Output: String</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Currency String Formatter")]
    public class CurrencyStringFormatter : DataProvider
    {
        /// <summary>
        ///     ISO code of culture to format the value with.
        ///     If empty the current culture will be used.
        /// </summary>
        [Tooltip(
            "ISO code of culture to format the value with. If empty the current culture will be used.")]
        public DataBinding CultureName;

        /// <summary>
        ///     Data value to use.
        /// </summary>
        public DataBinding Data;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var amount = this.Data.GetValue<float>();
                var cultureName = this.CultureName.GetValue<string>();
                var culture = string.IsNullOrEmpty(cultureName)
                    ? null
                    : CultureInfo.GetCultureInfo(cultureName);

                // Fallback to current culture.
                if (culture == null)
                {
                    culture = CultureInfo.CurrentCulture;
                }

                return amount.ToString("C", culture);
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Data);
            this.RemoveBinding(this.CultureName);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Data);
            this.AddBinding(this.CultureName);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}