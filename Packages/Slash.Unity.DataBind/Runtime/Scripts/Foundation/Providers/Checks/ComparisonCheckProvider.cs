// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ComparisonCheckProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Checks
{
    using System;
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;

    /// <summary>
    ///     Check to compare two comparable data values.
    /// </summary>
    public class ComparisonCheck : IDataProvider<bool>
    {
        private readonly ComparisonType comparisonType;

        private readonly IDataProvider firstProvider;

        private readonly IDataProvider secondProvider;

        private bool value;

        /// <inheritdoc />
        public ComparisonCheck(IDataProvider firstProvider, IDataProvider secondProvider,
            ComparisonType comparisonType)
        {
            this.firstProvider = firstProvider;
            this.firstProvider.ValueChanged += this.OnProviderValueChanged;
            this.secondProvider = secondProvider;
            this.secondProvider.ValueChanged += this.OnProviderValueChanged;
            this.comparisonType = comparisonType;

            // Initial value
            this.UpdateValue();
        }

        /// <inheritdoc />
        public event ValueChangedDelegate ValueChanged;

        /// <inheritdoc />
        public bool IsInitialized
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        public bool Value
        {
            get
            {
                return this.value;
            }
            private set
            {
                if (value == this.value)
                {
                    return;
                }

                this.value = value;

                this.OnValueChanged();
            }
        }

        /// <inheritdoc />
        object IDataProvider.Value
        {
            get
            {
                return this.Value;
            }
        }

        private static bool Compare(object first, object second, ComparisonType comparisonType)
        {
            // Make sure first value is comparable.
            var firstConverted = first as IComparable;
            if (firstConverted == null)
            {
                return false;
            }

            // Convert second value to type of first.
            object secondConverted;
            if (!ReflectionUtils.TryConvertValue(second, first.GetType(), out secondConverted))
            {
                return false;
            }

            // Compare values.
            var newValue = false;
            switch (comparisonType)
            {
                case ComparisonType.Equal:
                    newValue = firstConverted.CompareTo(secondConverted) == 0;
                    break;

                case ComparisonType.GreaterThan:
                    newValue = firstConverted.CompareTo(secondConverted) > 0;
                    break;

                case ComparisonType.LessThan:
                    newValue = firstConverted.CompareTo(secondConverted) < 0;
                    break;
            }

            return newValue;
        }

        private void OnProviderValueChanged()
        {
            this.UpdateValue();
        }

        private void OnValueChanged()
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler();
            }
        }

        private void UpdateValue()
        {
            this.Value = Compare(this.firstProvider.Value, this.secondProvider.Value, this.comparisonType);
        }
    }

    /// <summary>
    ///     Check to compare two comparable data values.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Checks/[DB] Comparison Check")]
    public class ComparisonCheckProvider : DataProvider
    {
        /// <summary>
        ///     How to compare the data values.
        /// </summary>
        public ComparisonType Comparison;

        /// <summary>
        ///     First data value.
        /// </summary>
        public DataBinding First;

        /// <summary>
        ///     Second data value.
        /// </summary>
        public DataBinding Second;

        private ComparisonCheck comparisonCheck;

        /// <summary>
        ///     Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.comparisonCheck.Value;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.First);
            this.RemoveBinding(this.Second);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.First);
            this.AddBinding(this.Second);

            this.comparisonCheck = new ComparisonCheck(this.First, this.Second, this.Comparison);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }

    /// <summary>
    ///     How to compare the data values.
    /// </summary>
    public enum ComparisonType
    {
        /// <summary>
        ///     Checks if the first value is less than the second one.
        /// </summary>
        LessThan,

        /// <summary>
        ///     Checks if the first value is equal to the second one.
        /// </summary>
        Equal,

        /// <summary>
        ///     Checks if the first value is greater than the second one.
        /// </summary>
        GreaterThan
    }
}