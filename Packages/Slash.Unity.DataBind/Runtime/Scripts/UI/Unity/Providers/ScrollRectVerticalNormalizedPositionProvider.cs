namespace Slash.Unity.DataBind.UI.Unity.Providers
{
    using System.Collections;
    using Slash.Unity.DataBind.Foundation.Providers.Getters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Provides the vertical normalized position of the bound scroll rect.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Providers/[DB] Scroll Rect Vertical Normalized Position Provider")]
    public class ScrollRectVerticalNormalizedPositionProvider : ComponentDataProvider<ScrollRect, float>
    {
        /// <inheritdoc />
        protected override void AddListener(ScrollRect target)
        {
            target.onValueChanged.AddListener(this.OnValueChanged);

            // Trigger ValueChanged next frame (ScrollRect updates in LateUpdate).
            this.StartCoroutine(this.ValueChangedEndOfFrame());
        }

        /// <inheritdoc />
        protected override float GetValue(ScrollRect target)
        {
            return target.verticalNormalizedPosition;
        }

        /// <inheritdoc />
        protected override void RemoveListener(ScrollRect target)
        {
            target.onValueChanged.RemoveListener(this.OnValueChanged);
        }

        private void OnValueChanged(Vector2 newValue)
        {
            this.OnValueChanged();
        }

        private IEnumerator ValueChangedEndOfFrame()
        {
            yield return new WaitForEndOfFrame();
            this.OnValueChanged();
        }
    }
}