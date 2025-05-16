// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;
    using Slash.Unity.DataBind.Core.Data;

    /// <summary>
    ///     Base class for a data provider.
    /// </summary>
    public abstract class DataProvider : DataBindingOperator, IDataProvider
    {
        /// <inheritdoc />
        public event ValueChangedDelegate ValueChanged;

        /// <summary>
        ///     Current data value.
        /// </summary>
        public abstract object Value { get; }

        /// <summary>
        ///     Called when a value of the bindings of this operator changed.
        /// </summary>
        protected override void OnBindingValuesChanged()
        {
            this.UpdateValue();
        }

        /// <summary>
        ///     Should be called by a derived class if the value of the data provider changed.
        /// </summary>
        /// <param name="newValue">New value of this data provider.</param>
        [Obsolete("Use method without parameter instead")]
        protected void OnValueChanged(object newValue)
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler();
            }
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
        ///     Called when the value of the data provider should be updated.
        /// </summary>
        protected virtual void UpdateValue()
        {
        }
    }
}