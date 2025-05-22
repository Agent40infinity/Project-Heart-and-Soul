// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToggleIsOnGetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Getters
{
    using System;
    using Slash.Unity.DataBind.Foundation.Providers.Getters;

    using UnityEngine.UI;

    /// <summary>
    ///   Provides a boolean value if a toggle is on.
    /// </summary>
    [Obsolete("Use ToggleIsOnProvider instead")]
    public class ToggleIsOnGetter : ComponentSingleGetter<Toggle, bool>
    {
        #region Methods

        /// <summary>
        ///     Register listener at target to be informed if its value changed.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to add listener to.</param>
        protected override void AddListener(Toggle target)
        {
            target.onValueChanged.AddListener(this.OnToggleValueChanged);
        }

        /// <summary>
        ///     Derived classes should return the current value to set if this method is called.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to get value from.</param>
        /// <returns>Current value to set.</returns>
        protected override bool GetValue(Toggle target)
        {
            return target.isOn;
        }

        /// <summary>
        ///     Remove listener from target which was previously added in AddListener.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to remove listener from.</param>
        protected override void RemoveListener(Toggle target)
        {
            target.onValueChanged.RemoveListener(this.OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool newValue)
        {
            this.OnTargetValueChanged();
        }

        #endregion
    }
}