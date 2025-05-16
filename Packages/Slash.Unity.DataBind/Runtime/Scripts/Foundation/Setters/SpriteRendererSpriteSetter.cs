// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SpriteRendererSpriteSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Setter which sets the sprite value of a SpriteRenderer.
    ///   <para>Input: <see cref="Sprite" /></para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] SpriteRenderer Sprite Setter")]
    public class SpriteRendererSpriteSetter : ComponentSingleSetter<SpriteRenderer, Sprite>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(SpriteRenderer target, Sprite value)
        {
            target.sprite = value;
        }
    }
}