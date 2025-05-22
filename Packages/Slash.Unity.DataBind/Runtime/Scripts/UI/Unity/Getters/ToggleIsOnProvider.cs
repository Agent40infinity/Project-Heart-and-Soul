// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToggleIsOnGetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Getters
{
    using Slash.Unity.DataBind.Foundation.Providers.Getters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Provides a boolean value if a toggle is on.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Getters/[DB] Toggle IsOn Provider (Unity)")]
    public class ToggleIsOnProvider : ComponentDataProvider<Toggle, bool>
    {
        /// <inheritdoc />
        protected override void AddListener(Toggle target)
        {
            target.onValueChanged.AddListener(this.OnToggleValueChanged);
        }

        /// <inheritdoc />
        protected override bool GetValue(Toggle target)
        {
            return target.isOn;
        }

        /// <inheritdoc />
        protected override void RemoveListener(Toggle target)
        {
            target.onValueChanged.RemoveListener(this.OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool newValue)
        {
            this.OnTargetValueChanged();
        }
    }
}