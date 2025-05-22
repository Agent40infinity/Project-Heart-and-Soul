namespace Slash.Unity.DataBind.Foundation.Providers.Operations
{
    using UnityEngine;

    /// <summary>
    ///   Inverts a Vector3 data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Operations/[DB] Invert Vector3 Operation")]
    public class InvertVector3Operation : InvertOperation<Vector3>
    {
        /// <inheritdoc />
        protected override Vector3 Invert(Vector3 value)
        {
            return value * -1;
        }
    }
}