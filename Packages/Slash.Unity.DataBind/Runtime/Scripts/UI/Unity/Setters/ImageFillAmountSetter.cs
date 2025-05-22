// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImageFillAmountSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Set the fill amount of an Image depending on the string data value.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Image Fill Amount Setter (Unity)")]
    public class ImageFillAmountSetter : ComponentSingleSetter<Image, float>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Image target, float value)
        {
            target.fillAmount = value;
        }
    }
}