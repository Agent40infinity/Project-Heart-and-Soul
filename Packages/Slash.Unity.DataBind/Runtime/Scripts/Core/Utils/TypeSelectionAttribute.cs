// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeSelectionAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System;

    using UnityEngine;

    /// <summary>
    ///   Use this attribute on a Type field which should be set to a type that
    ///   derives from a specific class.
    /// </summary>
    public class TypeSelectionAttribute : PropertyAttribute
    {
        #region Fields

        /// <summary>
        ///   Base type the selected type has to derive from.
        /// </summary>
        public Type BaseType;

        #endregion
    }
}