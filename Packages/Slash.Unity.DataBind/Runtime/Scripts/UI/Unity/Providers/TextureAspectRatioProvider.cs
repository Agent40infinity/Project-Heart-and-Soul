// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextureAspectRatioProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Providers
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Provides the aspect ratio (width / height) of the specified texture.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Providers/[DB] Texture AspectRatio Provider")]
    public class TextureAspectRatioProvider : DataProvider
    {
        /// <summary>
        ///     Texture to get aspect ratio for.
        /// </summary>
        public DataBinding Texture;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var texture = this.Texture.GetValue<Texture>();
                return texture != null ? texture.width * 1.0f / texture.height : 0;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Texture);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Texture);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}