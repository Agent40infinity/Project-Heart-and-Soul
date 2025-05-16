namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine;

    /// <summary>
    ///   Sets if a canvas group is interactable depending on a boolean data value.
    ///   <para>Input: Boolean</para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Canvas Group Interactable Setter (Unity)")]
    public class CanvasGroupInteractableSetter : ComponentSingleSetter<CanvasGroup, bool>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(CanvasGroup target, bool value)
        {
            target.interactable = value;
        }
    }
}