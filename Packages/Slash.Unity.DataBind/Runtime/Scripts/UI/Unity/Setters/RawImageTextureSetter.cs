// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawImageTextureSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Setter which sets the texture value of a RawImage component.
    ///     <para>Input: <see cref="Texture" /></para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] RawImage Texture Setter (Unity)")]
    public class RawImageTextureSetter : ComponentSingleSetter<RawImage, Texture>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(RawImage target, Texture value)
        {
            target.texture = value;
        }
    }
}