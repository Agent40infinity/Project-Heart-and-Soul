// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanSwitch.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Switches
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Data provider which chooses one of two options depending on a provided boolean value.
    ///     <para>Input: Boolean (Switch).</para>
    ///     <para>Output: Object (Chosen data).</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Switches/[DB] Boolean Switch")]
    public class BooleanSwitch : DataProvider
    {
        /// <summary>
        ///     Data to use if switch is false.
        /// </summary>
        [Tooltip("Data to use if switch is false.")]
        public DataBinding OptionFalse;

        /// <summary>
        ///     Data to use if switch is true.
        /// </summary>
        [Tooltip("Data to use if switch is true.")]
        public DataBinding OptionTrue;

        /// <summary>
        ///     Switch to decide which option to use.
        /// </summary>
        [Tooltip("Switch to decide which option to use.")]
        public DataBinding Switch;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                // Get value of switch.
                var switchValue = this.Switch.GetValue<bool>();
                return switchValue ? this.OptionTrue.Value : this.OptionFalse.Value;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Switch);
            this.RemoveBinding(this.OptionTrue);
            this.RemoveBinding(this.OptionFalse);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Switch);
            this.AddBinding(this.OptionTrue);
            this.AddBinding(this.OptionFalse);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}