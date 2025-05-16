// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Texture2DToSpriteConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using UnityEngine;

    /// <summary>
    ///   Converts a raw 2d texture to a sprite.
    ///   <para>Input: <see cref="Texture2D" /></para>
    ///   <para>Output: <see cref="Sprite" /></para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Converters/[DB] Texture2D to Sprite Converter")]
    public class Texture2DToSpriteConverter : DataConverter<Texture2D, Sprite>
    {
        #region Fields

        /// <summary>
        ///   Pivot of converted sprite.
        /// </summary>
        public Vector2 Pivot;

        /// <summary>
        ///   Specific rectangle from the texture to use.
        ///   Only considered if UseTextureRect == false.
        /// </summary>
        [Tooltip("Specific rectangle from the texture to use. Only considered if UseTextureRect == false.")]
        public Rect Rect;

        /// <summary>
        ///   Indicates if the complete texture should be used.
        /// </summary>
        [Tooltip("Indicates if the complete texture should be used.")]
        public bool UseTextureRect = true;

        #endregion

        #region Methods

        /// <summary>
        ///   Called when the specified value should be converted.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Converted value.</returns>
        protected override Sprite Convert(Texture2D value)
        {
            if (value == null)
            {
                return null;
            }
            return Sprite.Create(
                value,
                this.UseTextureRect ? new Rect(0, 0, value.width, value.height) : this.Rect,
                this.Pivot);
        }

        #endregion
    }
}