// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataNode.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
    using System;

    /// <summary>
    ///     Interface of a data node in a data tree.
    /// </summary>
    public interface IDataNode
    {
        /// <summary>
        ///     Called when the value of this node changed.
        /// </summary>
        event Action<object> ValueChanged;

        /// <summary>
        ///     Type of data.
        /// </summary>
        Type DataType { get; }

        /// <summary>
        ///     Name of node.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        ///     Current value.
        /// </summary>
        object Value { get; }

        /// <summary>
        ///     Returns the descendant node at the specified path.
        /// </summary>
        /// <param name="path">Path to return node for.</param>
        /// <returns>Descendant node at the specified path.</returns>
        IDataNode FindDescendant(string path);

        /// <summary>
        ///     Sets a new value for the data node.
        /// </summary>
        /// <param name="newValue">Value to set.</param>
        void SetValue(object newValue);
    }
}