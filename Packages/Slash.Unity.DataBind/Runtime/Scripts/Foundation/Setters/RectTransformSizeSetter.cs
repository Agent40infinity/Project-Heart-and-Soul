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
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] RectTransform Size Setter")]
    public class RectTransformSizeSetter : ComponentSingleSetter<RectTransform, Vector2>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(RectTransform target, Vector2 value)
        {
            target.sizeDelta = value;
        }
    }
}