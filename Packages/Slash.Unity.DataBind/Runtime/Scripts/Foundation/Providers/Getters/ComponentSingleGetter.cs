// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentSingleGetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    ///   Base class for a getter for a component which modifies a single data value.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to get value from.</typeparam>
    /// <typeparam name="TData">Type of data which is modified.</typeparam>
    [Obsolete("Use ComponentDataProvider instead and set the context data with a ContextDataUpdater")]
    public abstract class ComponentSingleGetter<TComponent, TData> : DataProvider, ISerializationCallbackReceiver
        where TComponent : Component
    {
        /// <summary>
        ///   Path to value in data context.
        /// </summary>
        [ContextPath(Filter = ~ContextMemberFilter.Methods | ~ContextMemberFilter.Contexts)]
        public string Path;

        /// <summary>
        ///   Binding that provides the target component.
        /// </summary>
        public DataBinding TargetBinding;

        /// <summary>
        ///   Target component.
        /// </summary>
        [FormerlySerializedAs("Target")]
        [SerializeField]
        [HideInInspector]
        private TComponent constantTarget;

        /// <summary>
        ///   Cache for contexts and master paths.
        /// </summary>
        private DataContextNodeConnector nodeConnector;

        /// <summary>
        ///   Current target component to get data value from.
        /// </summary>
        private TComponent target;

        /// <summary>
        ///   Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.target != null ? this.GetValue(this.target) : default(TData);
            }
        }

        /// <summary>
        ///   Current target component to get data value from.
        /// </summary>
        private TComponent Target
        {
            set
            {
                if (value == this.target)
                {
                    return;
                }

                if (this.target != null)
                {
                    this.RemoveListener(this.target);
                }

                this.target = value;

                if (this.target != null)
                {
                    this.AddListener(this.target);
                }
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.TargetBinding);
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            this.TargetBinding.ValueChanged -= this.OnTargetChanged;
            this.Target = null;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            this.TargetBinding.ValueChanged += this.OnTargetChanged;
            if (this.TargetBinding.IsInitialized)
            {
                this.OnTargetChanged();
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.nodeConnector = new DataContextNodeConnector(DataBindRunner.Instance.DataContextNodeConnectorInitializer, this, this.Path);
            this.AddBinding(this.TargetBinding);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        public void OnAfterDeserialize()
        {
            if (!this.IsTargetBindingSet())
            {
#if UNITY_EDITOR
                if (this.constantTarget == null)
                {
                    return;
                }
#endif
                this.TargetBinding = new DataBinding
                {
                    Type = DataBindingType.Reference,
                    Reference = this.constantTarget
                };
                this.constantTarget = null;
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        public void OnBeforeSerialize()
        {
        }

        /// <summary>
        ///   Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public override void OnContextChanged()
        {
            base.OnContextChanged();

            if (this.nodeConnector == null)
            {
                return;
            }

            this.nodeConnector.OnHierarchyChanged();

            // Update value.
            this.UpdateDataValue();
        }

        /// <summary>
        ///   Register listener at target to be informed if its value changed.
        ///   The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to add listener to.</param>
        protected abstract void AddListener(TComponent target);

        /// <summary>
        ///   Derived classes should return the current value to set if this method is called.
        ///   The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to get value from.</param>
        /// <returns>Current value to set.</returns>
        protected abstract TData GetValue(TComponent target);

        /// <summary>
        ///   Has to be called by derived classes when the value may have changed.
        /// </summary>
        protected void OnTargetValueChanged()
        {
            this.UpdateDataValue();
            this.OnValueChanged();
        }

        /// <summary>
        ///   Remove listener from target which was previously added in AddListener.
        ///   The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to remove listener from.</param>
        protected abstract void RemoveListener(TComponent target);

        /// <summary>
        ///   Unity callback.
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
        ///   Unity callback.
        /// </summary>
        protected override void Start()
        {
            base.Start();

            // Initial update of data value.
            this.UpdateDataValue();

            // Inform that value changed.
            this.OnValueChanged();
        }

        /// <summary>
        ///   Called when the value of the data provider should be updated.
        /// </summary>
        protected override void UpdateValue()
        {
            // Not required, data comes from presentation side.
        }

        private bool IsTargetBindingSet()
        {
            return
                !(this.TargetBinding == null
                  || this.TargetBinding.Type == DataBindingType.Context && string.IsNullOrEmpty(this.TargetBinding.Path));
        }

        private void OnTargetChanged()
        {
            var newValue = this.TargetBinding.GetValue<TComponent>();
            if (newValue == null || newValue is TComponent)
            {
                this.Target = (TComponent)newValue;
            }
            else
            {
                Debug.LogErrorFormat(this,
                    "Expected Target binding to be of type '{0}', but received object of type '{1}'. Setting Target to null.",
                    typeof(TComponent).Name, newValue.GetType().Name);
                this.Target = null;
            }
        }

        private void UpdateDataValue()
        {
            this.nodeConnector.SetValue(this.Value);
        }
    }
}