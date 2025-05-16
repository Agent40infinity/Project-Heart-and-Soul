namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using System;

    using UnityEngine;

    /// <summary>
    ///   Provides a single component of a <see cref="Vector3"/>.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Converters/[DB] Vector3 to Value Converter")]
    public class Vector3ToValueConverter : DataConverter<Vector3, float>
    {
        /// <summary>
        ///   Available Vector3 components to provide.
        /// </summary>
        public enum Vector3Component
        {
            /// <summary>
            ///   X value of the vector.
            /// </summary>
            X,

            /// <summary>
            ///   Y value of the vector.
            /// </summary>
            Y,

            /// <summary>
            ///   Z value of the vector.
            /// </summary>
            Z
        }

        /// <summary>
        ///   Component of vector to provide.
        /// </summary>
        [Tooltip("Component of vector to provide")]
        public Vector3Component Component;

        /// <inheritdoc />
        protected override float Convert(Vector3 value)
        {
            switch (this.Component)
            {
                case Vector3Component.X:
                    return value.x;
                case Vector3Component.Y:
                    return value.y;
                case Vector3Component.Z:
                    return value.z;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}