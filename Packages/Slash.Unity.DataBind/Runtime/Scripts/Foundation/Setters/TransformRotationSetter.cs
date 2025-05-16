// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformRotationSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace Slash.Unity.DataBind.Foundation.Setters
{
    /// <summary>
    ///     Sets the rotation of a transform depending on a data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Transform Rotation Setter")]
    public class TransformRotationSetter : ComponentSingleSetter<Transform, Quaternion>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Transform target, Quaternion value)
        {
            target.rotation = value;
        }
    }
}