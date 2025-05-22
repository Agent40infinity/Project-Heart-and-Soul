// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryLookup.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Lookups
{
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    /// <summary>
    ///     Looks up a value from a data dictionary by its key.
    /// </summary>
    public class DictionaryLookup : DataProvider
    {
        private DataDictionary dataDictionary;

        /// <summary>
        ///     Default value if key wasn't found in dictionary.
        /// </summary>
        public string DefaultValue;

        /// <summary>
        ///     Dictionary to get value from.
        /// </summary>
        public DataBinding Dictionary;

        /// <summary>
        ///     Key to get value for.
        /// </summary>
        public DataBinding Key;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                if (this.DataDictionary == null)
                {
                    return string.IsNullOrEmpty(this.DefaultValue) ? null : this.DefaultValue;
                }

                var key = this.Key.GetValue(this.DataDictionary.KeyType);

                object value;
                if (!this.DataDictionary.TryGetValue(key, out value))
                {
                    ReflectionUtils.TryConvertValue(this.DefaultValue, this.DataDictionary.ValueType, out value);
                }

                return value;
            }
        }

        private DataDictionary DataDictionary
        {
            get
            {
                return this.dataDictionary;
            }
            set
            {
                if (value == this.dataDictionary)
                {
                    return;
                }

                if (this.dataDictionary != null)
                {
                    this.dataDictionary.CollectionChanged -= this.OnDataDictionaryChanged;
                }

                this.dataDictionary = value;

                if (this.dataDictionary != null)
                {
                    this.dataDictionary.CollectionChanged += this.OnDataDictionaryChanged;
                }

                this.UpdateValue();
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Key);
            this.RemoveBinding(this.Dictionary);
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();
            this.Dictionary.ValueChanged -= this.OnDictionaryChanged;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();
            this.Dictionary.ValueChanged += this.OnDictionaryChanged;
            this.DataDictionary = this.Dictionary.GetValue<DataDictionary>();
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Key);
            this.AddBinding(this.Dictionary);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }

        private void OnDataDictionaryChanged()
        {
            this.UpdateValue();
        }

        private void OnDictionaryChanged()
        {
            this.DataDictionary = this.Dictionary.GetValue<DataDictionary>();
        }
    }
}