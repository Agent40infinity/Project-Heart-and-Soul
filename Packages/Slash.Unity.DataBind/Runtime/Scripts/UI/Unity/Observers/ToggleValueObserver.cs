namespace Slash.Unity.DataBind.UI.Unity.Observers
{
    using Slash.Unity.DataBind.Foundation.Observers;
    using UnityEngine.UI;

    /// <summary>
    ///   Observer for the value of a <see cref="Toggle"/>.
    /// </summary>
    public class ToggleValueObserver : ComponentDataObserver<Toggle, bool>
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