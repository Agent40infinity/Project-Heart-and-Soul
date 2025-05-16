namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using UnityEngine;

    /// <summary>
    ///   Converts euler angles to a quaternion
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Converters/[DB] Euler Angles to Quaternion Converter")]
    public class EulerAnglesToQuaternionConverter : DataConverter<Vector3, Quaternion>
    {
        /// <inheritdoc />
        protected override Quaternion Convert(Vector3 value)
        {
            return Quaternion.Euler(value);
        }
    }
}