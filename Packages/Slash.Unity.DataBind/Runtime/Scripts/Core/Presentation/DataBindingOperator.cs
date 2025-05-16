// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBindingOperator.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using UnityEngine;

    /// <summary>
    ///     Base class of a behaviour that uses on one or more data bindings.
    /// </summary>
    public class DataBindingOperator : MonoBehaviour, IContextOperator
    {
        /// <summary>
        ///     Bindings this operator depends on.
        /// </summary>
        private readonly List<RegisteredDataBinding> bindings = new List<RegisteredDataBinding>();

        /// <summary>
        ///     Indicates if Init method was called.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        ///     Indicates if the provider listens to value changes of its bindings.
        /// </summary>
        private bool isMonitoringBindings;

        /// <summary>
        ///     Indicates if the data provider already holds a valid value.
        /// </summary>
        public bool IsInitialized
        {
            get
            {
                return this.isInitialized && (this.bindings == null ||
                                              this.bindings.All(binding => binding.IsInitialized));
            }
        }

        /// <summary>
        ///     Adds and initializes the specified binding.
        /// </summary>
        /// <param name="binding">Binding to add.</param>
        public void AddBinding(DataBinding binding)
        {
            var registeredDataBinding = new RegisteredDataBinding(binding, null);
            this.RegisterBinding(registeredDataBinding);
            this.bindings.Add(registeredDataBinding);
        }

        /// <summary>
        ///     Adds and initializes the specified binding and adds a callback that's
        ///     invoked when the value of that binding changes.
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="onValueChanged"></param>
        public void AddBinding(DataBinding binding, Action onValueChanged)
        {
            var registeredDataBinding = new RegisteredDataBinding(binding, onValueChanged);
            this.RegisterBinding(registeredDataBinding);
            this.bindings.Add(registeredDataBinding);
        }

        /// <summary>
        ///     Deinitializes the operator.
        ///     By default this method removes the added bindings.
        /// </summary>
        public virtual void Deinit()
        {
            this.RemoveAllBindings();
        }

        /// <summary>
        ///     Disable the operator.
        /// </summary>
        public virtual void Disable()
        {
            this.UnregisterFromValueChanges();
        }

        /// <summary>
        ///     Enable the operator.
        /// </summary>
        public virtual void Enable()
        {
            this.RegisterForValueChanges();
            var bindingsInitialized = this.bindings.All(binding => binding.IsInitialized);
            if (bindingsInitialized)
            {
                this.OnBindingValuesChanged();
            }
        }

        /// <summary>
        ///     Initializes the operator.
        ///     This method should be used to add bindings.
        /// </summary>
        public virtual void Init()
        {
        }

        /// <summary>
        ///     Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public virtual void OnContextChanged()
        {
            foreach (var binding in this.bindings)
            {
                binding.DataBinding.OnContextChanged();
            }
        }

        /// <summary>
        ///     Adds and initializes the specified bindings.
        /// </summary>
        /// <param name="newBindings">Bindings to add.</param>
        protected void AddBindings(IEnumerable<DataBinding> newBindings)
        {
            foreach (var binding in newBindings)
            {
                this.AddBinding(binding);
            }
        }

        /// <summary>
        ///     Unity callback.
        ///     Overwrite Init method in derived classes to add bindings and do other initialization stuff.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected void Awake()
        {
            this.Init();
            this.isInitialized = true;
        }

        /// <summary>
        ///     Called when a value of the bindings of this operator changed.
        /// </summary>
        protected virtual void OnBindingValuesChanged()
        {
        }

        /// <summary>
        ///     Unity callback.
        ///     Overwrite Deinit method in derived classes to remove bindings and do other deinitialization stuff.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected void OnDestroy()
        {
            this.Deinit();
        }

        /// <summary>
        ///     Unity callback.
        ///     Overwrite Disable method in derived classes to disable behaviour.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected void OnDisable()
        {
            this.Disable();
        }

        /// <summary>
        ///     Unity callback.
        ///     Overwrite Enable method in derived classes to enable behaviour.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected void OnEnable()
        {
            this.Enable();
        }

        /// <summary>
        ///     Removes and deinitializes all added bindings.
        /// </summary>
        protected void RemoveAllBindings()
        {
            foreach (var binding in this.bindings)
            {
                this.UnregisterBinding(binding);
            }

            this.bindings.Clear();
        }

        /// <summary>
        ///     Removes and deinitializes the specified binding.
        /// </summary>
        /// <param name="binding">Binding to remove.</param>
        protected void RemoveBinding(DataBinding binding)
        {
            var registeredDataBinding = this.bindings.FirstOrDefault(existingRegisteredDataBinding =>
                existingRegisteredDataBinding.DataBinding == binding);
            if (registeredDataBinding == null)
            {
                return;
            }

            this.UnregisterBinding(registeredDataBinding);
            this.bindings.Remove(registeredDataBinding);
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMemberHiearchy.Global")]
        protected virtual void Start()
        {
        }

        private void OnBindingValueChanged(RegisteredDataBinding binding)
        {
            var handler = binding.Callback;
            if (handler != null)
            {
                handler();
            }

            this.OnBindingValuesChanged();
        }

        private void RegisterBinding(RegisteredDataBinding binding)
        {
            // Init.
            binding.DataBinding.Init(DataBindRunner.Instance.DataContextNodeConnectorInitializer, this);

            if (this.isMonitoringBindings)
            {
                binding.StartMonitoring();
                binding.ValueChanged += this.OnBindingValueChanged;
            }
        }

        private void RegisterForValueChanges()
        {
            if (this.isMonitoringBindings)
            {
                return;
            }

            foreach (var binding in this.bindings)
            {
                binding.StartMonitoring();
                binding.ValueChanged += this.OnBindingValueChanged;
            }

            this.isMonitoringBindings = true;
        }

        private void UnregisterBinding(RegisteredDataBinding binding)
        {
            if (this.isMonitoringBindings)
            {
                binding.StopMonitoring();
                binding.ValueChanged -= this.OnBindingValueChanged;
            }

            // Deinit.
            binding.DataBinding.Deinit();
        }

        private void UnregisterFromValueChanges()
        {
            if (!this.isMonitoringBindings)
            {
                return;
            }

            foreach (var binding in this.bindings)
            {
                binding.StopMonitoring();
                binding.ValueChanged -= this.OnBindingValueChanged;
            }

            this.isMonitoringBindings = false;
        }

        private class RegisteredDataBinding
        {
            public RegisteredDataBinding(DataBinding dataBinding, Action callback)
            {
                this.DataBinding = dataBinding;
                this.Callback = callback;
            }

            public event Action<RegisteredDataBinding> ValueChanged;

            public Action Callback { get; private set; }
            public DataBinding DataBinding { get; private set; }

            public bool IsInitialized
            {
                get
                {
                    return this.DataBinding.IsInitialized;
                }
            }

            public void StartMonitoring()
            {
                this.DataBinding.ValueChanged += this.OnValueChanged;
            }

            public void StopMonitoring()
            {
                this.DataBinding.ValueChanged -= this.OnValueChanged;
            }

            protected virtual void OnValueChanged()
            {
                var handler = this.ValueChanged;
                if (handler != null)
                {
                    handler(this);
                }
            }
        }
    }
}