// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainCameraProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provides the current main camera.
    /// </summary>
    public class MainCameraProvider : DataProvider
    {
        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return Camera.main;
            }
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}