namespace Slash.Unity.DataBind.Foundation.Providers.Smootheners
{
    using System;

    using UnityEngine;

    /// <summary>
    ///   Formats arguments by a specified format string to create a new string value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Smootheners/[DB] Float Smoothener")]
    public class FloatSmoothener : Smoothener<float>
    {
        /// <inheritdoc />
        protected override float DoStep(float value, float step, bool reverse)
        {
            return reverse ? value - step : value + step;
        }

        /// <inheritdoc />
        protected override float GetDifference(float valueA, float valueB)
        {
            return Math.Abs(valueA - valueB);
        }

        /// <inheritdoc />
        protected override bool IsLess(float valueA, float valueB)
        {
            return valueA < valueB;
        }

        /// <inheritdoc />
        protected override float GetVelocity(float value, float duration)
        {
            return value / duration;
        }

        /// <inheritdoc />
        protected override float GetStep(float velocity, float deltaTime)
        {
            return velocity * deltaTime;
        }
    }
}