// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformLocalPositionSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Sets the local position of a transform depending on a Vector3 data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] RectTransform Height Setter")]
    public class RectTransformHeightSetter : ComponentSingleSetter<RectTransform, float>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(RectTransform target, float value)
        {
            target.sizeDelta = new Vector2(target.sizeDelta.x, value);
        }
    }
}