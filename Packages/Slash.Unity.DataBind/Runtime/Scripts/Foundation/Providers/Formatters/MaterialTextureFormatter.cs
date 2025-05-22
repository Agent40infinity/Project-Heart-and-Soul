namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using UnityEngine;

    /// <summary>
    ///     Adjusts the texture with the specified name of a material depending on the bound value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Formatters/[DB] Material Texture Formatter")]
    public class MaterialTextureFormatter : Formatter<Material, Texture>
    {
        /// <summary>
        ///     Parameter name of texture.
        /// </summary>
        public string ShaderPropertyName;

        /// <inheritdoc />
        protected override bool UpdateTarget(Material target, Texture value)
        {
            target.SetTexture(this.ShaderPropertyName, value);
            return true;
        }
    }
}