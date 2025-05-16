// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PositionSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Sets the position of a game object depending on a Vector3 data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Position Setter")]
    [Obsolete("Use TransformPositionSetter if possible")]
    public class PositionSetter : GameObjectSingleSetter<Vector3>
    {
        #region Methods

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(Vector3 newValue)
        {
            this.Target.transform.position = newValue;
        }

        #endregion
    }
}