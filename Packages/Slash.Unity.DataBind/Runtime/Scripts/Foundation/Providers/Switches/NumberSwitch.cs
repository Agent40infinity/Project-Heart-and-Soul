// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NumberSwitch.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Switches
{
    using System;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    ///   Contains the number to check against to choose this option.
    /// </summary>
    [Serializable]
    public class NumberSwitchOption : SwitchOption
    {
        #region Fields

        /// <summary>
        ///   Required number to choose this option.
        /// </summary>
        [Tooltip("Required number to choose this option.")]
        public int Number;

        #endregion
    }

    /// <summary>
    ///   Data provider which chooses from specified integers.
    ///   <para>Input: Number.</para>
    ///   <para>Output: Object (Value of chosen range).</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Switches/[DB] Number Switch")]
    public class NumberSwitch : RangeSwitch<NumberRangeOption>
    {
        #region Fields

        /// <summary>
        ///   Options to choose from.
        /// </summary>
        public NumberSwitchOption[] Options;

        #endregion

        #region Methods

        /// <summary>
        ///   Selects the option to use for the specified value.
        /// </summary>
        /// <param name="value">Value to get option for.</param>
        /// <returns>Option to use for the specified value.</returns>
        protected override SwitchOption SelectOption(object value)
        {
            int number;
            try
            {
                number = Convert.ToInt32(value);
            }
            catch (Exception)
            {
                return null;
            }

            if (this.Options == null)
            {
                return null;
            }

            // Return first valid range.
            return this.Options.FirstOrDefault(option => option.Number == number);
        }

        #endregion
    }
}