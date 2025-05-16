// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionalFormatter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Returns one of two values, depending on a specified condition.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Conditional Formatter")]
    public class ConditionalFormatter : DataProvider
    {
        /// <summary>
        ///     Condition to check to decide which value to use.
        /// </summary>
        public DataBinding Condition;

        /// <summary>
        ///     Value to use when condition is not fulfilled.
        /// </summary>
        public DataBinding FalseValue;

        /// <summary>
        ///     Value to use when condition is fulfilled.
        /// </summary>
        public DataBinding TrueValue;

        /// <summary>
        ///     Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                var condition = this.Condition.GetValue<bool>();
                return condition ? this.TrueValue.Value : this.FalseValue.Value;
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            // Add bindings.
            this.AddBinding(this.Condition);
            this.AddBinding(this.TrueValue);
            this.AddBinding(this.FalseValue);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            // TODO(co): Cache current value and check if really changed?
            this.OnValueChanged();
        }
    }
}