namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using UnityEngine;

    /// <summary>
    ///   Provides the shared mesh of a mesh filter.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Getters/[DB] Mesh Filter Shared Mesh Provider")]
    public class MeshFilterSharedMeshProvider : ComponentPullDataProvider<MeshFilter, Mesh>
    {
        /// <inheritdoc />
        protected override Mesh GetValue(MeshFilter target)
        {
            return target.sharedMesh;
        }
    }
}