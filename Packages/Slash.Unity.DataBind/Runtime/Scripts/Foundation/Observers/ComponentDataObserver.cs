// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComponentDataObserver.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Observers
{
    using Slash.Unity.DataBind.Core.Data;

    /// <summary>
    ///     Base class to observe value changes of a single data value of a component.
    /// </summary>
    public abstract class ComponentDataObserver<TComponent, TData> : IDataProvider<TData>
    {
        private TComponent target;

        /// <inheritdoc />
        public event ValueChangedDelegate ValueChanged;

        /// <inheritdoc />
        public bool IsInitialized
        {
            get
            {
                return this.target != null;
            }
        }

        /// <summary>
        ///     Current target component to get data value from.
        /// </summary>
        public TComponent Target
        {
            get
            {
                return this.target;
            }
            set
            {
                if (Equals(value, this.target))
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

                this.OnValueChanged();
            }
        }

        /// <summary>
        ///     Current data value.
        /// </summary>
        public object Value
        {
            get
            {
                return this.Target != null ? this.GetValue(this.Target) : default(TData);
            }
        }

        /// <inheritdoc />
        TData IDataProvider<TData>.Value
        {
            get
            {
                return (TData) this.Value;
            }
        }

        /// <summary>
        ///     Register listener at target to be informed if its value changed.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to add listener to.</param>
        protected abstract void AddListener(TComponent target);

        /// <summary>
        ///     Derived classes should return the current value to set if this method is called.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to get value from.</param>
        /// <returns>Current value to set.</returns>
        protected abstract TData GetValue(TComponent target);

        /// <summary>
        ///     Has to be called by derived classes when the value may have changed.
        /// </summary>
        protected void OnTargetValueChanged()
        {
            this.OnValueChanged();
        }

        /// <summary>
        ///     Should be called by a derived class if the value of the data provider changed.
        /// </summary>
        protected void OnValueChanged()
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        ///     Remove listener from target which was previously added in AddListener.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to remove listener from.</param>
        protected abstract void RemoveListener(TComponent target);
    }
}