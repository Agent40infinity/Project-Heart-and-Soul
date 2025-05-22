// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INodeValueObserver.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Slash.Unity.DataBind.Core.Data
{
    using System;

    /// <summary>
    ///   Interface for a value observer of a data node.
    /// </summary>
    public interface INodeValueObserver
    {
        /// <summary>
        ///   Called when the value of the node changed.
        /// </summary>
        event Action ValueChanged;
    }
}