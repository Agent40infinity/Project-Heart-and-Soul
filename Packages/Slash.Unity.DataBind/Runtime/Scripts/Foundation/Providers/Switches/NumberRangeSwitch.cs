namespace Slash.Unity.DataBind.Foundation.Providers.Switches
{
    using System;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    ///   Number range that contains the area for which the option should be used.
    /// </summary>
    [Serializable]
    public class NumberRangeOption : SwitchOption
    {
        #region Fields

        /// <summary>
        ///   Indicates if the maximum value is included in the range or if it's exclusive.
        /// </summary>
        [Tooltip("Indicates if the maximum value is included in the range or if it's exclusive.")]
        public bool InclusiveMax;

        /// <summary>
        ///   Maximum value (inclusive or exclusive, depends on InclusiveMax).
        /// </summary>
        [Tooltip("Maximum value (inclusive or exclusive, depends on InclusiveMax).")]
        public float Max;

        /// <summary>
        ///   Minimum value (inclusive).
        /// </summary>
        [Tooltip("Minimum value (inclusive).")]
        public float Min;

        #endregion
    }

    /// <summary>
    ///   Data provider which chooses from specified number ranges.
    ///   <para>Input: Number.</para>
    ///   <para>Output: Object (Value of chosen range).</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Switches/[DB] Number Range Switch")]
    public class NumberRangeSwitch : RangeSwitch<NumberRangeOption>
    {
        #region Fields

        /// <summary>
        ///   Ranges to choose from.
        /// </summary>
        public NumberRangeOption[] Ranges;

        #endregion

        #region Methods

        /// <summary>
        ///   Selects the option to use for the specified value.
        /// </summary>
        /// <param name="value">Value to get option for.</param>
        /// <returns>Option to use for the specified value.</returns>
        protected override SwitchOption SelectOption(object value)
        {
            float number;
            try
            {
                number = Convert.ToSingle(value);
            }
            catch (Exception)
            {
                return null;
            }

            if (this.Ranges == null)
            {
                return null;
            }

            // Return first valid range.
            return this.Ranges.FirstOrDefault(rangeOption => this.IsInRange(rangeOption, number));
        }

        private bool IsInRange(NumberRangeOption rangeOption, float number)
        {
            // Check if below min value.
            if (number < rangeOption.Min)
            {
                return false;
            }

            // Check if above (or equal) max value.
            if (!rangeOption.InclusiveMax && number >= rangeOption.Max || number > rangeOption.Max)
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}