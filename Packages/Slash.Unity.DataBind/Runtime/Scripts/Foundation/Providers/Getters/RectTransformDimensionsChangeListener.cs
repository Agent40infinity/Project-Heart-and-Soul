namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    ///     Listener for rect transform to indicate a dimension change.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class RectTransformDimensionsChangeListener : UIBehaviour
    {
        /// <summary>
        ///     Called when the dimensions of the RectTransform changed.
        /// </summary>
        public event Action DimensionsChanged;

        /// <inheritdoc />
        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            this.OnDimensionsChanged();
        }

        private void OnDimensionsChanged()
        {
            var handler = this.DimensionsChanged;
            if (handler != null)
            {
                handler();
            }
        }
    }
}