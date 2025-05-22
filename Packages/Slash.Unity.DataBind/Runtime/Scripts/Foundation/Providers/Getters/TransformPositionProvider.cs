namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using UnityEngine;

    /// <summary>
    ///   Provides the position of the target transform.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Getters/[DB] Transform Position Provider")]
    public class TransformPositionProvider : ComponentPullDataProvider<Transform, Vector3>
    {
        /// <inheritdoc />
        protected override Vector3 GetValue(Transform target)
        {
            return target.position;
        }
    }
}