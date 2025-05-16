// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseColorAlphaToColorConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Converts a base color to a color with an explicit alpha value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Base Color Alpha To Color Converter")]
    public class BaseColorAlphaToColorConverter : DataConverter<Color, Color>
    {
        /// <summary>
        ///     Data to use for alpha component of color.
        /// </summary>
        [Tooltip("Data to use for alpha component of color.")]
        public DataBinding Alpha;

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.Alpha);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.Alpha);
        }

        /// <inheritdoc />
        protected override Color Convert(Color value)
        {
            return new Color(value.r, value.g, value.b, this.Alpha.GetValue<float>());
        }
    }
}