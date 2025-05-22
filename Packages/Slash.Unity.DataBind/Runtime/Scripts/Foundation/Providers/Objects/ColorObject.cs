// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorObject.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Objects
{
    using UnityEngine;

    /// <summary>
    ///     Provides a plain color object.
    ///     <para>Output: Color.</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Objects/[DB] Color Object")]
    public class ColorObject : ConstantObjectProvider<Color>
    {
        /// <summary>
        ///     Color this provider holds.
        /// </summary>
        [Tooltip("Color this provider holds.")]
        public Color Color;

        /// <inheritdoc />
        public override Color ConstantValue
        {
            get
            {
                return this.Color;
            }
        }
    }
}