namespace Slash.Unity.DataBind.UI.Unity.Commands
{
    using Slash.Unity.DataBind.Foundation.Commands;

    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    ///   Command which is invoked when the element is started to be dragged.
    ///   Parameters:
    ///   - Pointer event data.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Commands/[DB] Begin Drag Command (Unity)")]
    public class BeginDragCommand : Command, IBeginDragHandler
    {
        /// <summary>
        ///   Called when this element is started to be dragged.
        /// </summary>
        /// <param name="eventData">Data of the pointer the drag is done with.</param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            this.InvokeCommand(eventData);
        }
    }
}