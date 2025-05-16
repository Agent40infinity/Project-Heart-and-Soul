// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AspectRatioFitterAspectRatioSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Setter which sets the aspect ratio of a AspectRatioFitter component.
    ///     <para>Input: <see cref="float" /></para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] AspectRatioFitter AspectRatio Setter (Unity)")]
    public class AspectRatioFitterAspectRatioSetter : ComponentSingleSetter<AspectRatioFitter, float>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(AspectRatioFitter target, float value)
        {
            target.aspectRatio = value;
        }
    }
}