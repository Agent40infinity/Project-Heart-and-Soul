// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogicalBoolOperation.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Operations
{
    using System.Linq;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Connects data values by a logical operation.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Logical Bool Formatter")]
    public class LogicalBoolOperation : DataProvider
    {
        /// <summary>
        ///     Which logical operation to perform.
        /// </summary>
        public enum LogicalOperator
        {
            /// <summary>
            ///     Uses AND to connect the data values.
            /// </summary>
            And,

            /// <summary>
            ///     Uses OR to connect the data values.
            /// </summary>
            Or
        }

        /// <summary>
        ///     Data values to connect.
        /// </summary>
        public DataBinding[] Arguments = new DataBinding[2];

        /// <summary>
        ///     Which logical operation to perform.
        /// </summary>
        [Tooltip("Which logical operation to perform?")]
        public LogicalOperator Operator;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var newValue = this.Operator == LogicalOperator.And
                    ? this.Arguments.All(binding => binding.GetValue<bool>())
                    : this.Arguments.Any(binding => binding.GetValue<bool>());
                return newValue;
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            // Add bindings.
            foreach (var binding in this.Arguments)
            {
                this.AddBinding(binding);
            }
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}