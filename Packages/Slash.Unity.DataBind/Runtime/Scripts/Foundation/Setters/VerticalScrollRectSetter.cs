namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Sets the y position of a Scroll Rect to save the previous scroll position upon closure.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Vertical Scroll Rect Setter")]
    public class VerticalScrollRectSetter : ComponentSingleSetter<RectTransform, float>
    {
        // Used to set the y position of a scroll rect.
        protected override void UpdateTargetValue(RectTransform target, float value)
        {
            target.localPosition = new Vector2(target.localPosition.x, value);
        }
    }
}