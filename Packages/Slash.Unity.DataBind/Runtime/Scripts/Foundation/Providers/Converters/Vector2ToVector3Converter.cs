// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Vector2ToVector3Converter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using System;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Converts a <see cref="Vector2" /> to a <see cref="Vector3" /> object.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Converters/[DB] Vector2 to Vector3 Converter")]
    public class Vector2ToVector3Converter : DataConverter<Vector2, Vector3>
    {
        /// <summary>
        ///     Available component mappings.
        /// </summary>
        public enum ComponentMappings
        {
            /// <summary>
            ///   Uses X and Y of Vector2 as X and Y of Vector3
            /// </summary>
            XY,

            /// <summary>
            ///   Uses X and Y of Vector2 as X and Z of Vector3
            /// </summary>
            XZ
        }

        /// <summary>
        ///     Data to use for additional component of vector.
        /// </summary>
        [Tooltip("Data to use for additional component of vector.")]
        public DataBinding AdditionalValue;

        /// <summary>
        ///     How to map 2D components to 3D vector.
        /// </summary>
        [Tooltip("How to map 2D components to 3D vector")]
        public ComponentMappings ComponentMapping;

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();

            this.RemoveBinding(this.AdditionalValue);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();

            this.AddBinding(this.AdditionalValue);
        }

        /// <inheritdoc />
        protected override Vector3 Convert(Vector2 value)
        {
            var additionalValue = this.AdditionalValue.GetValue<float>();
            switch (this.ComponentMapping)
            {
                case ComponentMappings.XY:
                    return new Vector3(value.x, value.y, additionalValue);
                case ComponentMappings.XZ:
                    return new Vector3(value.x, additionalValue, value.y);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}