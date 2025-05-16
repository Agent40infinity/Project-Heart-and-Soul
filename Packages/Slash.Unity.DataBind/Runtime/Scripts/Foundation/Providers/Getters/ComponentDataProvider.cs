// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentValueProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using Slash.Unity.DataBind.Core.Presentation;
using UnityEngine;

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    /// <summary>
    ///     Base class for a provider of a single value from a component.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to get value from.</typeparam>
    /// <typeparam name="TData">Type of data which is provided.</typeparam>
    public abstract class ComponentDataProvider<TComponent, TData> : DataProvider
        where TComponent : Component
    {
        /// <summary>
        ///     Target component.
        /// </summary>
        [DataTypeHintGenericType]
        public DataBinding TargetBinding;

        /// <summary>
        ///   Current target component to get data value from.
        /// </summary>
        public TComponent Target
        {
            get
            {
                return this.TargetBinding.GetValue<TComponent>();
            }
        }

        /// <summary>
        ///     Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.Target != null ? this.GetValue(this.Target) : default(TData);
            }
        }

        /// <summary>
        ///     Register listener at target to be informed if its value changed.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to add listener to.</param>
        protected abstract void AddListener(TComponent target);

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.TargetBinding);
        }

        /// <summary>
        ///     Derived classes should return the current value to set if this method is called.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to get value from.</param>
        /// <returns>Current value to set.</returns>
        protected abstract TData GetValue(TComponent target);

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.TargetBinding);
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            if (this.Target != null)
            {
                this.RemoveListener(this.Target);
            }
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            if (this.Target != null)
            {
                this.AddListener(this.Target);
            }
        }

        /// <summary>
        ///     Has to be called by derived classes when the value may have changed.
        /// </summary>
        protected void OnTargetValueChanged()
        {
            this.OnValueChanged();
        }

        /// <summary>
        ///     Remove listener from target which was previously added in AddListener.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to remove listener from.</param>
        protected abstract void RemoveListener(TComponent target);

        /// <summary>
        ///     Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "VirtualMemberNeverOverriden.Global")]
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
        ///     Unity callback.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            // Inform that value changed.
            this.OnValueChanged();
        }

        /// <summary>
        ///     Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            // Not required, data comes from presentation side.
        }

        private bool IsTargetBindingSet()
        {
            return !(this.TargetBinding == null ||
                     this.TargetBinding.Type == DataBindingType.Context && string.IsNullOrEmpty(this.TargetBinding.Path));
        }
    }
}