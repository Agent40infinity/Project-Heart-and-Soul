// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityEventTrigger.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Triggers
{
    using System;
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///     Raises Unity events whenever a specified context trigger is invoked.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Triggers/[DB] Unity Event Trigger")]
    public class UnityEventTrigger : MonoBehaviour, IContextOperator
    {
        /// <summary>
        ///     Path of trigger that initiates the unity event.
        /// </summary>
        [ContextPath(Filter = ContextMemberFilter.Triggers | ContextMemberFilter.Recursive)]
        public string Path;

        /// <summary>
        ///     Unity event fired when trigger is invoked.
        /// </summary>
        public TriggerInvokedEvent TriggerInvoked;

        private DataContextNodeConnector nodeConnector;

        private DataTrigger trigger;

        private DataTrigger Trigger
        {
            set
            {
                if (value == this.trigger)
                {
                    return;
                }

                if (this.trigger != null)
                {
                    this.trigger.Invoked -= this.OnTriggerInvoked;
                }

                this.trigger = value;

                if (this.trigger != null)
                {
                    this.trigger.Invoked += this.OnTriggerInvoked;
                }
            }
        }

        /// <summary>
        ///     Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public void OnContextChanged()
        {
            if (this.nodeConnector != null)
            {
                this.nodeConnector.OnHierarchyChanged();
            }
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected virtual void Awake()
        {
            this.nodeConnector = new DataContextNodeConnector(DataBindRunner.Instance.DataContextNodeConnectorInitializer, this, this.Path);
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected void OnDestroy()
        {
            this.Trigger = null;

            this.nodeConnector.SetValueListener(null);
            this.nodeConnector = null;
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected virtual void OnDisable()
        {
            this.nodeConnector.SetValueListener(null);
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected virtual void OnEnable()
        {
            var value = this.nodeConnector.SetValueListener(this.OnObjectValueChanged);
            if (this.nodeConnector.IsInitialized)
            {
                this.OnObjectValueChanged(value);
            }
        }

        /// <summary>
        ///     Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected virtual void OnObjectValueChanged(object newValue)
        {
            this.Trigger = newValue as DataTrigger;
        }

        private void OnTriggerInvoked()
        {
            this.TriggerInvoked.Invoke(this.nodeConnector.Context);
        }

        /// <summary>
        ///     Event to fire when trigger was invoked.
        /// </summary>
        [Serializable]
        public class TriggerInvokedEvent : UnityEvent<object>
        {
        }
    }
}