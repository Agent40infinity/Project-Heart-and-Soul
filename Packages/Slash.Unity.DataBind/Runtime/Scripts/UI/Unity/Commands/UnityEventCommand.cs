// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityEventCommand.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Slash.Unity.DataBind.Foundation.Commands;

    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///   Base class for a command that monitors a specific mono behaviour to trigger itself.
    /// </summary>
    /// <typeparam name="TBehaviour">Type of mono behaviour this command monitors.</typeparam>
    public abstract class UnityEventCommandBase<TBehaviour> : Command
        where TBehaviour : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Target to work with.
        /// </summary>
        public TBehaviour Target;

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            if (this.Target == null)
            {
                this.Target = this.GetComponent<TBehaviour>();
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected virtual void OnDisable()
        {
            if (this.Target == null)
            {
                return;
            }

            this.RemoveListeners(this.Target);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected virtual void OnEnable()
        {
            if (this.Target == null)
            {
                return;
            }

            this.RegisterListeners(this.Target);
        }

        /// <summary>
        ///   Called when the observed event occured.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected virtual void OnEvent()
        {
            this.InvokeCommand();
        }

        /// <summary>
        ///   Called when the command should add listeners to the specified target to be informed when
        ///   about an event that would trigger the command.
        /// </summary>
        /// <param name="target">Target to add listeners to.</param>
        protected abstract void RegisterListeners(TBehaviour target);

        /// <summary>
        ///   Called when the command should remove its listeners from the specified target.
        /// </summary>
        /// <param name="target">Target to remove listeners from.</param>
        protected abstract void RemoveListeners(TBehaviour target);

        /// <summary>
        ///   Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
        protected virtual void Reset()
        {
            if (this.Target == null)
            {
                this.Target = this.GetComponent<TBehaviour>();
            }
        }

        #endregion
    }

    /// <summary>
    ///   Base class for a command which is called on a Unity event.
    /// </summary>
    /// <typeparam name="TBehaviour">Type of mono behaviour to observe for event.</typeparam>
    /// <typeparam name="TEventData1">Type of first event data send with the command.</typeparam>
    /// <typeparam name="TEventData2">Type of second event data send with the command.</typeparam>
    public abstract class UnityEventCommand<TBehaviour, TEventData1, TEventData2> : UnityEventCommandBase<TBehaviour>
        where TBehaviour : MonoBehaviour
    {
        #region Methods

        /// <summary>
        ///   Returns the event from the specified target to observe.
        /// </summary>
        /// <param name="target">Target behaviour to get event from.</param>
        /// <returns>Event from the specified target to observe.</returns>
        protected virtual UnityEvent<TEventData1, TEventData2> GetEvent(TBehaviour target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Returns the events from the specified target to observe.
        /// </summary>
        /// <param name="target">Target behaviour to get event from.</param>
        /// <returns>Events from the specified target to observe.</returns>
        protected virtual IEnumerable<UnityEvent<TEventData1, TEventData2>> GetEvents(TBehaviour target)
        {
            yield return this.GetEvent(target);
        }

        /// <summary>
        ///   Called when the command should add listeners to the specified target to be informed when
        ///   about an event that would trigger the command.
        /// </summary>
        /// <param name="target">Target to add listeners to.</param>
        protected override void RegisterListeners(TBehaviour target)
        {
            var unityEvents = this.GetEvents(target);
            foreach (var unityEvent in unityEvents)
            {
                unityEvent.AddListener(this.OnEvent);
            }
        }

        /// <summary>
        ///   Called when the command should remove its listeners from the specified target.
        /// </summary>
        /// <param name="target">Target to remove listeners from.</param>
        protected override void RemoveListeners(TBehaviour target)
        {
            var unityEvents = this.GetEvents(target);
            foreach (var unityEvent in unityEvents)
            {
                unityEvent.RemoveListener(this.OnEvent);
            }
        }

        private void OnEvent(TEventData1 arg1, TEventData2 arg2)
        {
            this.InvokeCommand(arg1, arg2);
        }

        #endregion
    }

    /// <summary>
    ///   Base class for a command which is called on a Unity event.
    /// </summary>
    /// <typeparam name="TBehaviour">Type of mono behaviour to observe for event.</typeparam>
    /// <typeparam name="TEventData">Type of event data send with the command.</typeparam>
    public abstract class UnityEventCommand<TBehaviour, TEventData> : UnityEventCommandBase<TBehaviour>
        where TBehaviour : MonoBehaviour
    {
        #region Methods

        /// <summary>
        ///   Returns the event from the specified target to observe.
        /// </summary>
        /// <param name="target">Target behaviour to get event from.</param>
        /// <returns>Event from the specified target to observe.</returns>
        protected virtual UnityEvent<TEventData> GetEvent(TBehaviour target)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Returns the events from the specified target to observe.
        /// </summary>
        /// <param name="target">Target behaviour to get event from.</param>
        /// <returns>Events from the specified target to observe.</returns>
        protected virtual IEnumerable<UnityEvent<TEventData>> GetEvents(TBehaviour target)
        {
            yield return this.GetEvent(target);
        }

        /// <summary>
        ///   Called when the command should add listeners to the specified target to be informed when
        ///   about an event that would trigger the command.
        /// </summary>
        /// <param name="target">Target to add listeners to.</param>
        protected override void RegisterListeners(TBehaviour target)
        {
            var unityEvents = this.GetEvents(target);
            foreach (var unityEvent in unityEvents)
            {
                unityEvent.AddListener(this.OnEvent);
            }
        }

        /// <summary>
        ///   Called when the command should remove its listeners from the specified target.
        /// </summary>
        /// <param name="target">Target to remove listeners from.</param>
        protected override void RemoveListeners(TBehaviour target)
        {
            var unityEvents = this.GetEvents(target);
            foreach (var unityEvent in unityEvents)
            {
                unityEvent.RemoveListener(this.OnEvent);
            }
        }

        /// <summary>
        ///   Called when an the event on the target occurred that this command is listening to.
        ///   By default this will invoke the command with the received event data, but derived commands may modify the event data first.
        /// </summary>
        /// <param name="eventData">Data send with the event.</param>
        protected virtual void OnEvent(TEventData eventData)
        {
            this.InvokeCommand(eventData);
        }

        #endregion
    }

    /// <summary>
    ///   Base class for a command which is called on a Unity event.
    /// </summary>
    /// <typeparam name="TBehaviour">Type of mono behaviour to observe for event.</typeparam>
    public abstract class UnityEventCommand<TBehaviour> : UnityEventCommandBase<TBehaviour>
        where TBehaviour : MonoBehaviour
    {
        #region Methods

        /// <summary>
        ///   Returns the event from the specified target to observe.
        /// </summary>
        /// <param name="target">Target behaviour to get event from.</param>
        /// <returns>Event from the specified target to observe.</returns>
        protected abstract UnityEvent GetEvent(TBehaviour target);

        /// <summary>
        ///   Returns the events from the specified target to observe.
        /// </summary>
        /// <param name="target">Target behaviour to get event from.</param>
        /// <returns>Events from the specified target to observe.</returns>
        protected virtual IEnumerable<UnityEvent> GetEvents(TBehaviour target)
        {
            yield return this.GetEvent(target);
        }

        /// <summary>
        ///   Called when the command should add listeners to the specified target to be informed when
        ///   about an event that would trigger the command.
        /// </summary>
        /// <param name="target">Target to add listeners to.</param>
        protected override void RegisterListeners(TBehaviour target)
        {
            var unityEvents = this.GetEvents(target);
            foreach (var unityEvent in unityEvents)
            {
                unityEvent.AddListener(this.OnEvent);
            }
        }

        /// <summary>
        ///   Called when the command should remove its listeners from the specified target.
        /// </summary>
        /// <param name="target">Target to remove listeners from.</param>
        protected override void RemoveListeners(TBehaviour target)
        {
            var unityEvents = this.GetEvents(target);
            foreach (var unityEvent in unityEvents)
            {
                unityEvent.RemoveListener(this.OnEvent);
            }
        }

        #endregion
    }
}