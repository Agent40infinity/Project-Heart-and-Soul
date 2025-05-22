// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentDataSynchronizer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Synchronizers
{
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;

    /// <summary>
    ///     Non-generic base class for a component data synchronizer.
    /// </summary>
    public abstract class ComponentDataSynchronizer<TComponent> : DataBindingOperator
        where TComponent : Component
    {
        /// <summary>
        ///     Path to value in data context.
        /// </summary>
        [ContextPath(Filter = ~ContextMemberFilter.Methods)]
        public string Path;

        /// <summary>
        ///     Target component.
        /// </summary>
        [DataTypeHintGenericType]
        public DataBinding TargetBinding;

        /// <summary>
        ///     Node to get the data from a context.
        /// </summary>
        private DataContextNodeConnector dataContextNodeConnector;

        /// <summary>
        ///     Current target component to get data value from.
        /// </summary>
        public TComponent Target
        {
            get { return this.TargetBinding.GetValue<TComponent>(); }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.TargetBinding);

            if (this.dataContextNodeConnector != null)
            {
                this.dataContextNodeConnector.SetValueListener(null);
                this.dataContextNodeConnector = null;
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.TargetBinding);

            this.dataContextNodeConnector = new DataContextNodeConnector(DataBindRunner.Instance.DataContextNodeConnectorInitializer, this, this.Path);
            var initialValue = this.dataContextNodeConnector.SetValueListener(this.OnContextValueChanged);
            if (this.dataContextNodeConnector.IsInitialized)
            {
                this.SetComponentValue(initialValue);
            }
        }

        /// <inheritdoc />
        public override void OnContextChanged()
        {
            base.OnContextChanged();

            if (this.dataContextNodeConnector != null)
            {
                this.dataContextNodeConnector.OnHierarchyChanged();
            }
        }

        /// <summary>
        ///     Has to be called by derived classes when the value of the component has changed.
        /// </summary>
        protected void OnComponentValueChanged(object newComponentValue)
        {
            this.dataContextNodeConnector.SetValue(newComponentValue);
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected virtual void Reset()
        {
            if (!this.IsTargetBindingSet())
            {
                this.TargetBinding = new DataBinding
                {
                    Type = DataBindingType.Reference,
                    Reference = this.GetComponent<TComponent>()
                };
            }
        }

        /// <summary>
        ///     Sets a new data value on the target component.
        /// </summary>
        /// <param name="newContextValue">Data value to set.</param>
        protected abstract void SetComponentValue(object newContextValue);

        private bool IsTargetBindingSet()
        {
            return !(this.TargetBinding == null ||
                     this.TargetBinding.Type == DataBindingType.Context &&
                     string.IsNullOrEmpty(this.TargetBinding.Path));
        }

        private void OnContextValueChanged(object newContextValue)
        {
            // Update component value.
            this.SetComponentValue(newContextValue);
        }
    }

    /// <summary>
    ///     Generic base class for a component data synchronizer.
    /// </summary>
    /// <typeparam name="TComponent">Type of component this synchronizer is for.</typeparam>
    /// <typeparam name="TData">Type of data to work with.</typeparam>
    public abstract class ComponentDataSynchronizer<TComponent, TData> : ComponentDataSynchronizer<TComponent>
        where TComponent : Component
    {
        /// <inheritdoc />
        protected override void SetComponentValue(object newContextValue)
        {
            var target = this.Target;
            if (target != null)
            {
                TData newValue;
                if (!ReflectionUtils.TryConvertValue(newContextValue, out newValue))
                {
                    Debug.LogWarningFormat(this, "Couldn't convert new context value '{0}' of type '{1}' to type '{2}'",
                        newContextValue, newContextValue != null ? newContextValue.GetType() : null, typeof(TData));
                    return;
                }

                this.SetTargetValue(target, newValue);
            }
        }

        /// <summary>
        ///     Sets a new value on the target component.
        /// </summary>
        /// <param name="target">Target component to set value for.</param>
        /// <param name="newContextValue">Data value to set.</param>
        protected abstract void SetTargetValue(TComponent target, TData newContextValue);
    }
}