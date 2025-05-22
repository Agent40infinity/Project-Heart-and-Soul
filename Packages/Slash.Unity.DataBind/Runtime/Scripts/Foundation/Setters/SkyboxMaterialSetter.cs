namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///     Sets the material of a skybox.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Skybox Material Setter")]
    public class SkyboxMaterialSetter : ComponentSingleSetter<Skybox, Material>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Skybox target, Material value)
        {
            target.material = value;
        }
    }
}