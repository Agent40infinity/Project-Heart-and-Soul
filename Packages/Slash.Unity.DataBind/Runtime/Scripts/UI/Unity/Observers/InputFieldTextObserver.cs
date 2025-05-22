// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFieldTextObserver.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.Unity.DataBind.UI.Unity.Observers
{
    using Slash.Unity.DataBind.Foundation.Observers;
    using UnityEngine.UI;

    /// <summary>
    ///   Observes value changes of the text of an input field.
    /// </summary>
    public class InputFieldTextObserver : ComponentDataObserver<InputField, string>
    {
        /// <summary>
        ///   If set, the ValueChanged event is only dispatched when editing of the input field ended (i.e. input field left).
        /// </summary>
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