namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///     Sets the mesh of a mesh filter.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] MeshFilter Mesh Setter")]
    public class MeshFilterMeshSetter : ComponentSingleSetter<MeshFilter, Mesh>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(MeshFilter target, Mesh value)
        {
            target.mesh = value;
        }
    }
}