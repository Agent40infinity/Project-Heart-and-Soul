// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValuesToColorConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Converts 4 single numbers to a <see cref="Color" /> object.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Converters/[DB] Values to Color Converter")]
    public class ValuesToColorConverter : DataProvider
    {
        /// <summary>
        ///     Data to use for alpha component of color.
        /// </summary>
        [Tooltip("Data to use for alpha component of color.")]
        public DataBinding ValueA;

        /// <summary>
        ///     Data to use for blue component of color.
        /// </summary>
        [Tooltip("Data to use for blue component of color.")]
        public DataBinding ValueB;

        /// <summary>
        ///     Data to use for green component of color.
        /// </summary>
        [Tooltip("Data to use for green component of color.")]
        public DataBinding ValueG;

        /// <summary>
        ///     Data to use for red component of color.
        /// </summary>
        [Tooltip("Data to use for red component of color.")]
        public DataBinding ValueR;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return new Color(this.ValueR.GetValue<float>(), this.ValueG.GetValue<float>(),
                    this.ValueB.GetValue<float>(), this.ValueA.GetValue<float>());
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.ValueR);
            this.RemoveBinding(this.ValueG);
            this.RemoveBinding(this.ValueB);
            this.RemoveBinding(this.ValueA);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.ValueR);
            this.AddBinding(this.ValueG);
            this.AddBinding(this.ValueB);
            this.AddBinding(this.ValueA);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}