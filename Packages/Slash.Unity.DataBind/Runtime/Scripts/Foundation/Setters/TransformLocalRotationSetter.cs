// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformLocalRotationSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Sets the local rotation of a transform depending on a data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Transform Local Rotation Setter")]
    public class TransformLocalRotationSetter : ComponentSingleSetter<Transform, Quaternion>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Transform target, Quaternion value)
        {
            target.localRotation = value;
        }
    }
}