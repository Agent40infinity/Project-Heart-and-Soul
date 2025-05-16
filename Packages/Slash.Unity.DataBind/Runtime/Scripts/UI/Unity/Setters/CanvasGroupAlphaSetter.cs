namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine;

    /// <summary>
    ///   Set the alpha value of a canvas group depending on a float data value.
    ///   <para>Input: Number</para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Canvas Group Alpha Setter (Unity)")]
    public class CanvasGroupAlphaSetter : ComponentSingleSetter<CanvasGroup, float>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(CanvasGroup target, float value)
        {
            target.alpha = value;
        }
    }
}