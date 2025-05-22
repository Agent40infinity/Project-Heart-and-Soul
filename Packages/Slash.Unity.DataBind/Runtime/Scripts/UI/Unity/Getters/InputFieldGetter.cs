// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFieldGetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Getters
{
    using System;
    using Slash.Unity.DataBind.Foundation.Providers.Getters;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Gets the value of an input field.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Getters/[DB(Obsolete)] Input Field Value Getter (Unity)")]
    [Obsolete("Use InputFieldTextProvider instead")]
    public class InputFieldGetter : ComponentSingleGetter<InputField, string>
    {
        #region Methods

        /// <summary>
        ///     Register listener at target to be informed if its value changed.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to add listener to.</param>
        protected override void AddListener(InputField target)
        {
            target.onValueChanged.AddListener(this.OnTargetValueChanged);
        }

        /// <summary>
        ///     Derived classes should return the current value to set if this method is called.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to get value from.</param>
        /// <returns>Current value to set.</returns>
        protected override string GetValue(InputField target)
        {
            return target.text;
        }

        /// <summary>
        ///     Remove listener from target which was previously added in AddListener.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to remove listener from.</param>
        protected override void RemoveListener(InputField target)
        {
            target.onValueChanged.RemoveListener(this.OnTargetValueChanged);
        }

        private void OnTargetValueChanged(string newValue)
        {
            this.OnTargetValueChanged();
        }

        #endregion
    }
}