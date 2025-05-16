// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentSingleSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Diagnostics.CodeAnalysis;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    ///     Base class for a setter for a component.
    /// </summary>
    /// <typeparam name="TComponent">Type of component.</typeparam>
    /// <typeparam name="TData">Type of data to set.</typeparam>
    public abstract class ComponentSingleSetter<TComponent, TData> : SingleSetter<TData>, ISerializationCallbackReceiver
        where TComponent : Component
    {
        /// <summary>
        ///     Target to get the data from.
        /// </summary>
        [DataTypeHintGenericType]
        public DataBinding TargetBinding;

        /// <summary>
        ///     For backward compatibility.
        /// </summary>
        [FormerlySerializedAs("Target")]
        [SerializeField]
        [HideInInspector]
        private TComponent constantTarget;

        private TComponent target;

        /// <summary>
        ///     Current target to get data from.
        /// </summary>
        protected TComponent Target
        {
            get
            {
                return this.target;
            }
            private set
            {
                if (value == this.target)
                {
                    return;
                }

                this.target = value;

                if (this.Data.IsInitialized)
                {
                    this.OnObjectValueChanged();
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
            this.AddBinding(this.TargetBinding);
        }

        /// <summary>
        ///     <para>
        ///         Implement this method to receive a callback after Unity deserializes your object.
        ///     </para>
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
        ///     <para>
        ///         Implement this method to receive a callback before Unity serializes your object.
        ///     </para>
        /// </summary>
        public void OnBeforeSerialize()
        {
        }

        /// <inheritdoc />
        protected sealed override void OnValueChanged(TData newValue)
        {
            if (this.Target != null)
            {
                this.UpdateTargetValue(this.Target, newValue);
            }
        }

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
                    Reference = this.constantTarget ?? this.GetComponent<TComponent>()
                };
            }
        }

        /// <summary>
        ///     Called when data value on target should be updated.
        /// </summary>
        /// <param name="target">Target to update.</param>
        /// <param name="value">Value to set on target.</param>
        protected abstract void UpdateTargetValue(TComponent target, TData value);

        private bool IsTargetBindingSet()
        {
            return
                !(this.TargetBinding == null
                  || this.TargetBinding.Type == DataBindingType.Context &&
                  string.IsNullOrEmpty(this.TargetBinding.Path));
        }

        private void OnTargetChanged()
        {
            var newValue = this.TargetBinding.Value;
            if (newValue == null || newValue is TComponent)
            {
                this.Target = (TComponent) newValue;
            }
            else
            {
                Debug.LogErrorFormat(this,
                    "Expected Target binding to be of type '{0}', but received object of type '{1}'. Setting Target to null.",
                    typeof(TComponent).Name, newValue.GetType().Name);
                this.Target = null;
            }
        }
    }
}