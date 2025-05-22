namespace Slash.Unity.DataBind.UI.Unity.Synchronizers
{
    using Slash.Unity.DataBind.Foundation.Synchronizers;
    using Slash.Unity.DataBind.UI.Unity.Observers;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Synchronizer for the text of a <see cref="InputField"/>.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Synchronizers/[DB] Input Field Text Synchronizer (Unity)")]
    public class InputFieldTextSynchronizer : ComponentDataSynchronizer<InputField, string>
    {
        /// <summary>
        ///     If set, the ValueChanged event is only dispatched when editing of the input field ended (i.e. input field left).
        /// </summary>
        [Tooltip(
            "If set, the ValueChanged event is only dispatched when editing of the input field ended (i.e. input field left).")]
        public bool OnlyUpdateValueOnEndEdit;

        private InputFieldTextObserver observer;

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            if (this.observer != null)
            {
                this.observer.ValueChanged -= this.OnObserverValueChanged;
                this.observer = null;
            }
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            var target = this.Target;
            if (target != null)
            {
                this.observer = new InputFieldTextObserver
                {
                    OnlyUpdateValueOnEndEdit = this.OnlyUpdateValueOnEndEdit,
                    Target = target
                };
                this.observer.ValueChanged += this.OnObserverValueChanged;
            }
        }

        /// <inheritdoc />
        protected override void SetTargetValue(InputField target, string newContextValue)
        {
            target.text = newContextValue;
        }

        private void OnObserverValueChanged()
        {
            this.OnComponentValueChanged(this.Target.text);
        }
    }
}