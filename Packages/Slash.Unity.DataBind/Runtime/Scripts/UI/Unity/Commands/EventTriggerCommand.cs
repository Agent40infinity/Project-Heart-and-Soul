// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventTriggerCommand.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Commands
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;

    /// <summary>
    ///   Command which is invoked when an <see cref="EventTrigger" /> fires one of its triggers.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Commands/[DB] Event Trigger Command (Unity)")]
    public class EventTriggerCommand : UnityEventCommand<EventTrigger, BaseEventData>
    {
        /// <summary>
        ///   Returns the events from the specified target to observe.
        /// </summary>
        /// <param name="target">Target behaviour to get event from.</param>
        /// <returns>Events from the specified target to observe.</returns>
        protected override IEnumerable<UnityEvent<BaseEventData>> GetEvents(EventTrigger target)
        {
            return target.triggers.Select(trigger => trigger.callback as UnityEvent<BaseEventData>);
        }

        /// <summary>
        ///   Called when an the event on the target occurred that this command is listening to.
        ///   By default this will invoke the command with the received event data, but derived commands may modify the event data
        ///   first.
        /// </summary>
        /// <param name="eventData">Data send with the event.</param>
        protected override void OnEvent(BaseEventData eventData)
        {
            base.OnEvent();
        }
    }
}