// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFieldTextProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Getters
{
    using Slash.Unity.DataBind.Foundation.Providers.Getters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Provides the text of an input field.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Getters/[DB] Input Field Text Provider (Unity)")]
    public class InputFieldTextProvider : ComponentDataProvider<InputField, string>
    {
        /// <summary>
        ///   If set, the ValueChanged event is only dispatched when editing of the input field ended (i.e. input field left).
        /// </summary>
        [Tooltip("If set, the ValueChanged event is only dispatched when editing of the input field ended (i.e. input field left).")]
        public bool OnlyUpdateValueOnEndEdit;

        /// <inheritdoc />
        protected override void AddListener(InputField target)
        {
            target.onValueChanged.AddListener(this.OnInputFieldValueChanged);
            target.onEndEdit.AddListener(this.OnInputFieldEndEdit);
        }

        /// <inheritdoc />
        protected override string GetValue(InputField target)
        {
            return target.text;
        }

        /// <inheritdoc />
        protected override void RemoveListener(InputField target)
        {
            target.onValueChanged.RemoveListener(this.OnInputFieldValueChanged);
            target.onEndEdit.RemoveListener(this.OnInputFieldEndEdit);
        }

        private void OnInputFieldEndEdit(string newValue)
        {
            if (this.OnlyUpdateValueOnEndEdit)
            {
                this.OnTargetValueChanged();
            }
        }

        private void OnInputFieldValueChanged(string newValue)
        {
            if (!this.OnlyUpdateValueOnEndEdit)
            {
                this.OnTargetValueChanged();
            }
        }
    }
}