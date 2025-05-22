namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using UnityEngine;

    /// <summary>
    ///   Provides the local position of the target transform.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Getters/[DB] Transform Local Position Provider")]
    public class TransformLocalPositionProvider : ComponentPullDataProvider<Transform, Vector3>
    {
        /// <inheritdoc />
        protected override Vector3 GetValue(Transform target)
        {
            return target.localPosition;
        }
    }
}