// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantDataProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Objects
{
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;

    /// <summary>
    ///   Data provider which always returns a constant object.
    /// </summary>
    /// <typeparam name="T">Type of object the data provider returns.</typeparam>
    public class ConstantObject<T> : IDataProvider<T>
    {
        private readonly T value;

        /// <inheritdoc />
        public ConstantObject(T value)
        {
            this.value = value;
        }

        /// <inheritdoc />
#pragma warning disable 67
        public event ValueChangedDelegate ValueChanged;
#pragma warning restore 67

        /// <inheritdoc />
        public bool IsInitialized
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public T Value
        {
            get
            {
                return this.value;
            }
        }

        /// <inheritdoc />
        object IDataProvider.Value
        {
            get
            {
                return this.value;
            }
        }
    }

    /// <summary>
    ///     Base class to provide a constant value.
    /// </summary>
    /// <typeparam name="T">Type of value the class provides.</typeparam>
    public abstract class ConstantObjectProvider<T> : DataProvider
    {
        private T currentValue;

        /// <summary>
        ///     Constant value to provide.
        /// </summary>
        public abstract T ConstantValue { get; }

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return this.currentValue;
            }
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            this.currentValue = this.ConstantValue;
            this.OnValueChanged();
        }

        /// <summary>
        ///     Unity callback.
        /// </summary>
        protected void Update()
        {
            // Check for changes of the constant value when in editor, so it is possible to
            // play around with the value in the inspector.
#if UNITY_EDITOR
            if (!Equals(this.ConstantValue, this.currentValue))
            {
                this.currentValue = this.ConstantValue;
                this.OnValueChanged();
            }
#endif
        }
    }
}