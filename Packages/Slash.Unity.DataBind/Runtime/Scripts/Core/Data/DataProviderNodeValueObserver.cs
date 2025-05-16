// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataProviderNodeValueObserver.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
    using System;

    /// <summary>
    ///     Provides the node value by using a DataProvider.
    /// </summary>
    public class DataProviderNodeValueObserver : INodeValueObserver
    {
        /// <summary>
        ///     Data provider to get informed if value changes.
        /// </summary>
        private readonly IDataProvider dataProvider;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="dataProvider">Data provider to get value from.</param>
        public DataProviderNodeValueObserver(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        /// <inheritdoc />
        public event Action ValueChanged
        {
            add
            {
                if (this.valueChanged == null)
                {
                    if (this.dataProvider != null)
                    {
                        this.dataProvider.ValueChanged += this.OnDataProviderValueChanged;
                    }
                }

                this.valueChanged += value;
            }
            remove
            {
                this.valueChanged -= value;

                if (this.valueChanged == null)
                {
                    if (this.dataProvider != null)
                    {
                        this.dataProvider.ValueChanged -= this.OnDataProviderValueChanged;
                    }
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        private event Action valueChanged;
        
        private void OnDataProviderValueChanged()
        {
            // Update cached value.
            this.OnValueChanged();
        }

        private void OnValueChanged()
        {
            var handler = this.valueChanged;
            if (handler != null)
            {
                handler();
            }
        }
    }
}