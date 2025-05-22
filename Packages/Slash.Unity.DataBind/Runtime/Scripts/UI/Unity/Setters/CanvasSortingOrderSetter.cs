namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using UnityEngine;

    /// <summary>
    ///     Sets the sorting order of a canvas.
    ///     <para>Input: Integer</para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Canvas Sorting Order Setter (Unity)")]
    public class CanvasSortingOrderSetter : ComponentSingleSetter<Canvas, int>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Canvas target, int value)
        {
            target.sortingOrder = value;
        }
    }
}