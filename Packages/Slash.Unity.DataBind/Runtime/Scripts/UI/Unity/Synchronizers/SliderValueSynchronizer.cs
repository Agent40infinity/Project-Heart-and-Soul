// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SliderValueSynchronizer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Synchronizers
{
    using Slash.Unity.DataBind.Foundation.Observers;
    using Slash.Unity.DataBind.Foundation.Synchronizers;
    using Slash.Unity.DataBind.UI.Unity.Observers;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Synchronizer for the value of a <see cref="Slider"/>.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Synchronizers/[DB] Slider Value Synchronizer (Unity)")]
    public class SliderValueSynchronizer : ComponentDataSynchronizer<Slider, float>
    {
        private ComponentDataObserver<Slider, float> observer;

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            if (this.observer != null)
            {
                this.observer.ValueChanged -= this.OnObserverValueChanged;
                this.observer = null;
            }
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            var target = this.Target;
            if (target != null)
            {
                this.observer = new SliderValueObserver { Target = target };
                this.observer.ValueChanged += this.OnObserverValueChanged;
            }
        }

        /// <inheritdoc />
        protected override void SetTargetValue(Slider target, float newContextValue)
        {
            target.value = newContextValue;
        }

        private void OnObserverValueChanged()
        {
            this.OnComponentValueChanged(this.Target.value);
        }
    }
}