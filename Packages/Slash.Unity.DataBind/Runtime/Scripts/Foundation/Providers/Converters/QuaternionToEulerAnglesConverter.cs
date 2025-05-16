namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using UnityEngine;

    /// <summary>
    ///   Converts a quaternion to euler angles.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Converters/[DB] Quaternion to Euler Angles Converter")]
    public class QuaternionToEulerAnglesConverter : DataConverter<Quaternion, Vector3>
    {
        /// <inheritdoc />
        protected override Vector3 Convert(Quaternion value)
        {
            return value.eulerAngles;
        }
    }
}