namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Sets the material of a mesh renderer.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] MeshRenderer Material Setter")]
    public class MeshRendererMaterialSetter : ComponentSingleSetter<MeshRenderer, Material>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(MeshRenderer target, Material value)
        {
            target.material = value;
        }
    }
}