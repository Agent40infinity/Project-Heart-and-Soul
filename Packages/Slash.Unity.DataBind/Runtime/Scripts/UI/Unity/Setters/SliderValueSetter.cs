// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SliderValueSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Set the value of a slider depending on the string data value.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Slider Value Setter (Unity)")]
    public class SliderValueSetter : ComponentSingleSetter<Slider, float>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Slider target, float value)
        {
            target.value = value;
        }
    }
}