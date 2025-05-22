// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InvertBoolOperation.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Operations
{
    using UnityEngine;

    /// <summary>
    ///   Inverts a boolean data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Operations/[DB] Invert Bool Operation")]
    public class InvertBoolOperation : InvertOperation<bool>
    {
        /// <inheritdoc />
        protected override bool Invert(bool value)
        {
            return !value;
        }
    }
}