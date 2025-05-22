// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransformRotationProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using UnityEngine;

    /// <summary>
    ///   Provides the local rotation of the target transform.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Getters/[DB] Transform Local Rotation Provider")]
    public class TransformLocalRotationProvider : ComponentPullDataProvider<Transform, Quaternion>
    {
        /// <inheritdoc />
        protected override Quaternion GetValue(Transform target)
        {
            return target.localRotation;
        }
    }
}