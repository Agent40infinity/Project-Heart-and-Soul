// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using System;
    using UnityEngine;

    /// <summary>
    ///     Base class for a setter of a single data value.
    /// </summary>
    public abstract class SingleSetter : DataBindingOperator
    {
        /// <summary>
        ///     Data to bind to.
        /// </summary>
        public DataBinding Data;

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
            this.Data.ValueChanged -= this.OnObjectValueChanged;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();
            this.Data.ValueChanged += this.OnObjectValueChanged;
            if (this.Data.IsInitialized)
            {
                this.OnObjectValueChanged();
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.Data);
        }

        /// <summary>
        ///     Called when the data binding value changed.
        /// </summary>
        protected virtual void OnObjectValueChanged()
        {
        }
    }

    /// <summary>
    ///     Generic base class for a single data setter of a specific type.
    /// </summary>
    /// <typeparam name="TData">Type of data to set.</typeparam>
    public abstract class SingleSetter<TData> : DataBindingOperator
    {
        /// <summary>
        ///     Data to bind to.
        /// </summary>
        [DataTypeHintGenericType]
        public DataBinding Data;

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
            this.Data.ValueChanged -= this.OnObjectValueChanged;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();
            this.Data.ValueChanged += this.OnObjectValueChanged;
            if (this.Data.IsInitialized)
            {
                this.OnObjectValueChanged();
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.Data);
        }

        /// <summary>
        ///     Called when the data binding value changed.
        /// </summary>
        protected virtual void OnObjectValueChanged()
        {
            var newValue = this.Data.Value;
            TData value;
            if (newValue is TData)
            {
                value = (TData) newValue;
            }
            else
            {
                try
                {
                    value = this.Data.GetValue<TData>();
                }
                catch (Exception e)
                {
                    Debug.LogWarning(
                        string.Format(
                            "Couldn't convert new value '{0}' to type '{1}', using default value: {2}",
                            newValue,
                            typeof(TData),
                            e.Message),
                        this);
                    value = default(TData);
                }
            }

            this.OnValueChanged(value);
        }

        /// <summary>
        ///     Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected abstract void OnValueChanged(TData newValue);
    }
}