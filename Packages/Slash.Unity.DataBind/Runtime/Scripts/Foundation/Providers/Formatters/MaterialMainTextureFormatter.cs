namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using UnityEngine;

    /// <summary>
    ///   Adjusts the main texture of a material depending on the bound value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Formatters/[DB] Material MainTexture Formatter")]
    public class MaterialMainTextureFormatter : Formatter<Material, Texture>
    {
        /// <inheritdoc />
        protected override bool UpdateTarget(Material target, Texture value)
        {
            if (value == target.mainTexture)
            {
                return false;
            }

            target.mainTexture = value;
            return true;
        }
    }
}