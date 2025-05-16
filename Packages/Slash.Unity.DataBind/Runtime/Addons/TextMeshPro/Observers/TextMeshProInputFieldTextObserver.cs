namespace Slash.Unity.DataBind.Addons.TextMeshPro.Observers
{
    using Slash.Unity.DataBind.Foundation.Observers;
    using TMPro;

    /// <summary>
    ///     Observes value changes of the text of an input field.
    /// </summary>
    public class TextMeshProInputFieldTextObserver : ComponentDataObserver<TMP_InputField, string>
    {
        /// <summary>
        ///     If set, the ValueChanged event is only dispatched when editing of the input field ended (i.e. input field left).
        /// </summary>
        public bool OnlyUpdateValueOnEndEdit;

        /// <inheritdoc />
        protected override void AddListener(TMP_InputField target)
        {
            target.onValueChanged.AddListener(this.OnInputFieldValueChanged);
            target.onEndEdit.AddListener(this.OnInputFieldEndEdit);
        }

        /// <inheritdoc />
        protected override string GetValue(TMP_InputField target)
        {
            return target.text;
        }

        /// <inheritdoc />
        protected override void RemoveListener(TMP_InputField target)
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