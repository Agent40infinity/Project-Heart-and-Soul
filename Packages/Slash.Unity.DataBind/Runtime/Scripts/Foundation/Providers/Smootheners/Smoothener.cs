// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Smoothener.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Smootheners
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Base class to smooth a data value.
    /// </summary>
    /// <typeparam name="TValue">Type of data to smooth.</typeparam>
    public abstract class Smoothener<TValue> : DataProvider
    {
        /// <summary>
        ///     Binding to get target value from.
        /// </summary>
        public DataBinding Data;

        /// <summary>
        ///     Indicates if a new target value can interrupt the current transition.
        ///     If true the current transition is completed before a new target value is used.
        /// </summary>
        [Tooltip(
            "Indicates if a new target value can interrupt the current transition. If true the current transition is completed before a new target value is used."
        )]
        public bool InterruptDuringTransition = true;

        /// <summary>
        ///     If set to smaller as 0, it is ignored. If set to 0 values are instant updated.
        /// </summary>
        [Tooltip("If set to smaller as 0, it is ignored. If set to 0 values are instant updated.")]
        public float MaxUpdateTime = -1f;

        /// <summary>
        ///     Velocity to change from current to target value (in change per second).
        /// </summary>
        [Tooltip("Velocity to change from current to target value (in change per second)")]
        public TValue Velocity;

        private TValue actualValue;

        private TValue targetValue;

        /// <summary>
        ///     Velocity of current transition.
        /// </summary>
        private TValue velocity;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return this.actualValue;
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();
            this.Data.ValueChanged -= this.OnDataValueChanged;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();
            this.actualValue = this.targetValue = this.Data.GetValue<TValue>();
            this.Data.ValueChanged += this.OnDataValueChanged;
        }

        /// <inheritdoc />
        public override void Init()
        {
            // Add bindings.
            this.AddBinding(this.Data);
        }

        /// <summary>
        ///     Does one step for the data value.
        /// </summary>
        /// <param name="value">Current value.</param>
        /// <param name="step">Step to take.</param>
        /// <param name="reverse">Indicates if the step should be forward or reverse.</param>
        /// <returns>New value.</returns>
        protected abstract TValue DoStep(TValue value, TValue step, bool reverse);

        /// <summary>
        ///     Returns the difference between two values.
        /// </summary>
        /// <param name="valueA">First value.</param>
        /// <param name="valueB">Second value.</param>
        /// <returns>Difference between two specified values.</returns>
        protected abstract TValue GetDifference(TValue valueA, TValue valueB);

        /// <summary>
        ///     Returns the step to take for the specified velocity and available time.
        /// </summary>
        /// <param name="velocity">Velocity to get step for.</param>
        /// <param name="deltaTime">Available time for step.</param>
        /// <returns>Step value for the specified velocity and available time.</returns>
        protected abstract TValue GetStep(TValue velocity, float deltaTime);

        /// <summary>
        ///     Returns the velocity required to process the specified amount in the specified duration.
        /// </summary>
        /// <param name="amount">Amount to process.</param>
        /// <param name="duration">Duration available to process (in s).</param>
        /// <returns>Velocity required to process the amount with (in 1/s).</returns>
        protected abstract TValue GetVelocity(TValue amount, float duration);

        /// <summary>
        ///     Indicates if value A is less than value B.
        /// </summary>
        /// <param name="valueA">First value.</param>
        /// <param name="valueB">Second value.</param>
        /// <returns>True if first value is less than second value; otherwise, false.</returns>
        protected abstract bool IsLess(TValue valueA, TValue valueB);

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected void Update()
        {
            if (this.IsTransitionActive())
            {
                var step = this.GetStep(this.velocity, Time.deltaTime);
                var difference = this.GetDifference(this.actualValue, this.targetValue);

                // Check if last step.
                if (this.IsLess(difference, step))
                {
                    this.actualValue = this.IsLess(this.actualValue, this.targetValue)
                        ? this.DoStep(this.actualValue, difference, false)
                        : this.DoStep(this.actualValue, difference, true);

                    if (!this.InterruptDuringTransition)
                    {
                        // Update target value (may have changed during transition).
                        this.UpdateTargetValue();
                    }
                }
                else
                {
                    this.actualValue = this.IsLess(this.actualValue, this.targetValue)
                        ? this.DoStep(this.actualValue, step, false)
                        : this.DoStep(this.actualValue, step, true);
                }

                this.OnValueChanged();
            }
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            // TODO(co): Cache current value and check if really changed?
            this.OnValueChanged();
        }

        private bool IsTransitionActive()
        {
            return !Equals(this.actualValue, this.targetValue);
        }

        private void OnDataValueChanged()
        {
            if (!this.InterruptDuringTransition && this.IsTransitionActive())
            {
                return;
            }

            this.UpdateTargetValue();
        }

        private void UpdateTargetValue()
        {
            var newTargetValue = this.Data.GetValue<TValue>();

            if (Equals(newTargetValue, this.targetValue))
            {
                return;
            }

            this.targetValue = newTargetValue;

            if (this.MaxUpdateTime == 0)
            {
                this.actualValue = this.targetValue;
                this.OnValueChanged();
            }
            else if (this.MaxUpdateTime < 0)
            {
                this.velocity = this.Velocity;
            }
            else
            {
                // Check if target value would be reached with specified velocity.
                var difference = this.GetDifference(this.targetValue, this.actualValue);
                var minVelocity = this.GetVelocity(difference, this.MaxUpdateTime);
                this.velocity = this.IsLess(minVelocity, this.Velocity) ? this.Velocity : minVelocity;
            }
        }
    }
}