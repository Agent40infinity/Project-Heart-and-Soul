// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RangeSwitch.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Switches
{
    using Slash.Unity.DataBind.Core.Presentation;

    /// <summary>
    ///     Base class for data providers which return different values depending on specified value ranges.
    /// </summary>
    /// <typeparam name="T">Type of ranges to check.</typeparam>
    public abstract class RangeSwitch<T> : DataProvider
        where T : SwitchOption
    {
        /// <summary>
        ///     Default data value to use if no option is valid.
        /// </summary>
        public DataBinding Default;

        /// <summary>
        ///     Data value to use as switch.
        /// </summary>
        public DataBinding Switch;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var value = this.Switch.Value;
                var option = this.SelectOption(value);
                if (option == null)
                {
                    return this.Default.Value;
                }

                // Init if not done.
                if (!option.Value.IsInitialized)
                {
                    option.Value.Init(DataBindRunner.Instance.DataContextNodeConnectorInitializer, this);
                }

                return option.Value.Value;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Switch);
            this.RemoveBinding(this.Default);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Switch);
            this.AddBinding(this.Default);
        }

        /// <summary>
        ///     Selects the option to use for the specified value.
        /// </summary>
        /// <param name="value">Value to get option for.</param>
        /// <returns>Option to use for the specified value.</returns>
        protected abstract SwitchOption SelectOption(object value);

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}