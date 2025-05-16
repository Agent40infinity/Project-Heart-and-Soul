namespace Slash.Unity.DataBind.Foundation.Providers.Smootheners
{
    using System;

    using UnityEngine;

    /// <summary>
    ///   Formats arguments by a specified format string to create a new string value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Smootheners/[DB] Long Smoothener")]
    public class LongSmoothener : Smoothener<long>
    {
        /// <inheritdoc />
        protected override long DoStep(long value, long step, bool reverse)
        {
            return reverse ? value - step : value + step;
        }

        /// <inheritdoc />
        protected override long GetDifference(long valueA, long valueB)
        {
            return Math.Abs(valueA - valueB);
        }

        /// <inheritdoc />
        protected override bool IsLess(long valueA, long valueB)
        {
            return valueA < valueB;
        }

        /// <inheritdoc />
        protected override long GetStep(long velocity, float deltaTime)
        {
            // TODO: With small deltas and small velocity this may finish the process much quicker than desired. 
            // A solution would be to accumulate the times and only process a step if it is not zero.
            return Mathf.CeilToInt(velocity * deltaTime);
        }

        /// <inheritdoc />
        protected override long GetVelocity(long amount, float duration)
        {
            return Mathf.CeilToInt(amount / duration);
        }
    }
}