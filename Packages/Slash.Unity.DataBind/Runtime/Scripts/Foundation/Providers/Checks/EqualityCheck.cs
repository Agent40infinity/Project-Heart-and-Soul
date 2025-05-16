// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualityCheck.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Checks
{
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;

    /// <summary>
    ///     Checks for equality of two data values.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Checks/[DB] Equality Check")]
    public class EqualityCheck : DataProvider
    {
        /// <summary>
        ///     First data value.
        /// </summary>
        public DataBinding First;

        /// <summary>
        ///     Second data value.
        /// </summary>
        public DataBinding Second;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var firstValue = this.First.Value;
                var secondValue = this.Second.Value;
                return ComparisonUtils.CheckValuesForEquality(firstValue, secondValue);
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            // Add bindings.
            this.AddBinding(this.First);
            this.AddBinding(this.Second);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}