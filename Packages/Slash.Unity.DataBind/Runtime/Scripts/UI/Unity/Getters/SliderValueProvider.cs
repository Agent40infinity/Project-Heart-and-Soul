// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SliderValueProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Getters
{
    using Slash.Unity.DataBind.Foundation.Providers.Getters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Provides the value of a slider.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Getters/[DB] Slider Value Provider (Unity)")]
    public class SliderValueProvider : ComponentDataProvider<Slider, float>
    {
        /// <inheritdoc />
        protected override void AddListener(Slider target)
        {
            target.onValueChanged.AddListener(this.OnTargetValueChanged);
        }

        /// <inheritdoc />
        protected override float GetValue(Slider target)
        {
            return target.value;
        }

        /// <inheritdoc />
        protected override void RemoveListener(Slider target)
        {
            target.onValueChanged.RemoveListener(this.OnTargetValueChanged);
        }

        private void OnTargetValueChanged(float newValue)
        {
            this.OnTargetValueChanged();
        }
    }
}