// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparisonUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    /// <summary>
    ///   Utility methods for comparing objects.
    /// </summary>
    public static class ComparisonUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Checks two objects for equality.
        ///   Tries to convert second value to the same type as first value.
        /// </summary>
        /// <param name="firstValue">First value to compare.</param>
        /// <param name="secondValue">Second value to compare.</param>
        /// <returns>True if both values can be converted to the same type and have an equal value; otherwise, false.</returns>
        public static bool CheckValuesForEquality(object firstValue, object secondValue)
        {
            if (firstValue == secondValue)
            {
                return true;
            }

            if (firstValue == null || secondValue == null)
            {
                return false;
            }

            // Change type of second value to compare.
            var firstValueType = firstValue.GetType();

            object secondValueConverted;
            return ReflectionUtils.TryConvertValue(secondValue, firstValueType, out secondValueConverted)
                   && Equals(firstValue, secondValueConverted);
        }

        #endregion
    }
}