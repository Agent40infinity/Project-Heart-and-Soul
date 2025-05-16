// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValuesToVector3Converter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Converts 3 single numbers to a <see cref="Vector3" /> object.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Converters/[DB] Values to Vector3 Converter")]
    public class ValuesToVector3Converter : DataProvider
    {
        /// <summary>
        ///     Data to use for x component of vector.
        /// </summary>
        [Tooltip("Data to use for x component of vector.")]
        public DataBinding ValueX;

        /// <summary>
        ///     Data to use for y component of vector.
        /// </summary>
        [Tooltip("Data to use for y component of vector.")]
        public DataBinding ValueY;

        /// <summary>
        ///     Data to use for z component of vector.
        /// </summary>
        [Tooltip("Data to use for z component of vector.")]
        public DataBinding ValueZ;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return new Vector3(this.ValueX.GetValue<float>(), this.ValueY.GetValue<float>(),
                    this.ValueZ.GetValue<float>());
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.ValueX);
            this.RemoveBinding(this.ValueY);
            this.RemoveBinding(this.ValueZ);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.ValueX);
            this.AddBinding(this.ValueY);
            this.AddBinding(this.ValueZ);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}