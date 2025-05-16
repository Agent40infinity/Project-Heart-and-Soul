// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringObject.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Objects
{
    using UnityEngine;

    /// <summary>
    ///     Provides a plain string object.
    ///     <para>Output: string.</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Objects/[DB] String Object")]
    public class StringObject : ConstantObjectProvider<string>
    {
        /// <summary>
        ///     Text this provider holds.
        /// </summary>
        [Tooltip("Text this provider holds.")]
        public string Text;

        /// <inheritdoc />
        public override string ConstantValue
        {
            get
            {
                return this.Text;
            }
        }
    }
}