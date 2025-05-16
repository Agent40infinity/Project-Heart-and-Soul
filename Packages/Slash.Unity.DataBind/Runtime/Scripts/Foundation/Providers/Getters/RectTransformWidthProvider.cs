namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using UnityEngine;

    /// <summary>
    ///     Provides the width of the target rect transform.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Getters/[DB] RectTransform Width Provider")]
    public class RectTransformWidthProvider : ComponentDataProvider<RectTransform, float>
    {
        /// <inheritdoc />
        protected override void AddListener(RectTransform target)
        {
            var listener = target.GetComponent<RectTransformDimensionsChangeListener>();
            if (listener == null)
            {
                listener = target.gameObject.AddComponent<RectTransformDimensionsChangeListener>();
            }

            listener.DimensionsChanged += this.OnDimensionsChanged;
        }

        /// <inheritdoc />
        protected override float GetValue(RectTransform target)
        {
            return target.sizeDelta.x;
        }

        /// <inheritdoc />
        protected override void RemoveListener(RectTransform target)
        {
            var listener = target.GetComponent<RectTransformDimensionsChangeListener>();
            if (listener != null)
            {
                listener.DimensionsChanged -= this.OnDimensionsChanged;
            }
        }

        private void OnDimensionsChanged()
        {
            this.OnTargetValueChanged();
        }
    }
}