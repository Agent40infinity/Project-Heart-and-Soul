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
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Transform Local Position Setter")]
    public class TransformLocalPositionSetter : ComponentSingleSetter<Transform, Vector3>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Transform target, Vector3 value)
        {
            target.localPosition = value;
        }
    }
}