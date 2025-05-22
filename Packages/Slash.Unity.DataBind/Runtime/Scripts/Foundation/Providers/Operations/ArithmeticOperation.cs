// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArithmeticOperation.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Operations
{
    using System;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    ///     Performs an arithmetic operation with the two data values.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Arithmetic Operation")]
    public class ArithmeticOperation : DataProvider
    {
        /// <summary>
        ///     Which arithmetic operation to perform.
        /// </summary>
        public enum ArithmeticOperationType
        {
            /// <summary>
            ///     No arithmetic operation, always 0.
            /// </summary>
            None,

            /// <summary>
            ///     Sums the data values.
            /// </summary>
            Add,

            /// <summary>
            ///     Subtracts the second data value from the first one.
            /// </summary>
            Sub,

            /// <summary>
            ///     Multiplies the data values.
            /// </summary>
            Multiply,

            /// <summary>
            ///     Divides the first data value by the second one.
            /// </summary>
            Divide,

            /// <summary>
            ///     Modulo the first data value by the second one.
            /// </summary>
            Modulo
        }

        /// <summary>
        ///     First data value.
        /// </summary>
        [FormerlySerializedAs("ArgumentA")]
        public DataBinding First;

        /// <summary>
        ///     Second data value.
        /// </summary>
        [FormerlySerializedAs("ArgumentB")]
        public DataBinding Second;

        /// <summary>
        ///     Which arithmetic operation to perform.
        /// </summary>
        [Tooltip("Which arithmetic operation to perform?")]
        public ArithmeticOperationType Type;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var firstValue = this.First.GetValue<float>();
                var secondValue = this.Second.GetValue<float>();

                var newValue = 0.0f;
                switch (this.Type)
                {
                    case ArithmeticOperationType.Add:
                        newValue = firstValue + secondValue;
                        break;
                    case ArithmeticOperationType.Sub:
                        newValue = firstValue - secondValue;
                        break;
                    case ArithmeticOperationType.Multiply:
                        newValue = firstValue * secondValue;
                        break;
                    case ArithmeticOperationType.Divide:
                        if (Math.Abs(secondValue) > 0)
                        {
                            newValue = firstValue / secondValue;
                        }
                        break;
                    case ArithmeticOperationType.Modulo:
                        if (Math.Abs(secondValue) > 0)
                        {
                            newValue = firstValue % secondValue;
                        }
                        break;
                }

                return newValue;
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