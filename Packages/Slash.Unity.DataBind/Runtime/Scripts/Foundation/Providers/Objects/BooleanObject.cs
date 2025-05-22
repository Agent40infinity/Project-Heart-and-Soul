// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanObject.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Objects
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Provides a plain boolean object.
    ///     <para>Output: Boolean.</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Objects/[DB] Boolean Object")]
    [DataTypeHintExplicit(typeof(bool))]
    public class BooleanObject : ConstantObjectProvider<bool>
    {
        /// <summary>
        ///     Boolean this provider holds.
        /// </summary>
        [Tooltip("Boolean this provider holds.")]
        public bool Boolean;

        /// <inheritdoc />
        public override bool ConstantValue
        {
            get
            {
                return this.Boolean;
            }
        }
    }
}