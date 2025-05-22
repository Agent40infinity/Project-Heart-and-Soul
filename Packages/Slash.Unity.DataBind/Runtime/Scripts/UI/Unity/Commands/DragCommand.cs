// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DragCommand.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Commands
{
    using Slash.Unity.DataBind.Foundation.Commands;

    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    ///   Command which is invoked when the element is dragged.
    ///   Parameters:
    ///   - Pointer event data.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Commands/[DB] Drag Command (Unity)")]
    public class DragCommand : Command, IDragHandler
    {
        /// <summary>
        ///   Called while this element is dragged.
        /// </summary>
        /// <param name="eventData">Data of the pointer the drag is done with.</param>
        public void OnDrag(PointerEventData eventData)
        {
            this.InvokeCommand(eventData);
        }
    }
}