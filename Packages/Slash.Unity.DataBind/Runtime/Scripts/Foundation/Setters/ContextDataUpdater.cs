// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextDataUpdater.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Collections;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;

    /// <summary>
    ///     Updates a data property of a context with the value of a data binding.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Context Data Updater")]
    public class ContextDataUpdater : DataBindingOperator
    {
        /// <summary>
        ///     Data to update context data from.
        /// </summary>
        public DataBinding Data;

        /// <summary>
        ///     Path to value to update in data context.
        /// </summary>
        [ContextPath(Filter = ~ContextMemberFilter.Methods | ~ContextMemberFilter.Contexts)]
        public string Path;

        private DataContextNodeConnector nodeConnector;

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.Data);
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            this.Data.ValueChanged -= this.OnDataChanged;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            this.Data.ValueChanged += this.OnDataChanged;
            if (this.Data.IsInitialized)
            {
                this.OnDataChanged();
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.nodeConnector = new DataContextNodeConnector(DataBindRunner.Instance.DataContextNodeConnectorInitializer, this, this.Path);
            this.AddBinding(this.Data);
        }

        /// <inheritdoc />
        public override void OnContextChanged()
        {
            base.OnContextChanged();

            if (this.nodeConnector == null)
            {
                return;
            }

            this.nodeConnector.OnHierarchyChanged();

            if (this.isActiveAndEnabled)
            {
                // Delay data value update till end of frame as there may be other data bindings require a context update.
                this.StartCoroutine(this.UpdateDataValueOnEndOfFrame());
            }
        }

        private void OnDataChanged()
        {
            if (this.nodeConnector.IsInitialized)
            {
                this.UpdateDataValue(this.Data.Value);
            }
            else
            {
                // Delay data value update till end of frame to make sure node connector is initialized.
                this.StartCoroutine(this.UpdateDataValueOnEndOfFrame());
            }
        }

        private void UpdateDataValue(object value)
        {
            this.nodeConnector.SetValue(value);
        }

        private IEnumerator UpdateDataValueOnEndOfFrame()
        {
            yield return new WaitForEndOfFrame();

            // Update value.
            this.UpdateDataValue(this.Data.Value);
        }
    }
}