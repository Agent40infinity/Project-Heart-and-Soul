namespace Slash.Unity.DataBind.UI.Unity.Observers
{
    using Slash.Unity.DataBind.Foundation.Observers;
    using UnityEngine.UI;

    /// <summary>
    ///   Observer for the value of a <see cref="Slider"/>.
    /// </summary>
    public class SliderValueObserver : ComponentDataObserver<Slider, float>
    {
        /// <inheritdoc />
        protected override void AddListener(Slider target)
        {
            target.onValueChanged.AddListener(this.OnSliderValueChanged);
        }

        /// <inheritdoc />
        protected override float GetValue(Slider target)
        {
            return target.value;
        }

        /// <inheritdoc />
        protected override void RemoveListener(Slider target)
        {
            target.onValueChanged.RemoveListener(this.OnSliderValueChanged);
        }

        private void OnSliderValueChanged(float newValue)
        {
            this.OnTargetValueChanged();
        }
    }
}