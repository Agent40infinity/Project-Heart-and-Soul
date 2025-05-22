// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataTree.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
    using System;

    /// <summary>
    ///     Handles the data nodes as a tree structure and allows to access the data via a path.
    /// </summary>
    public class DataTree
    {
        private readonly IDataNode root;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="root">Root node of the tree.</param>
        public DataTree(IDataNode root)
        {
            this.root = root;
        }

        /// <summary>
        ///     Returns the value at the specified path.
        /// </summary>
        /// <param name="path">Path to get value for.</param>
        /// <exception cref="ArgumentException">Thrown if path is invalid for this tree.</exception>
        /// <returns>Current value at specified path.</returns>
        public object GetValue(string path)
        {
            var node = this.ResolvePath(path);
            return node.Value;
        }

        /// <summary>
        ///     Registers a callback at the specified path.
        /// </summary>
        /// <param name="path">Path to register for.</param>
        /// <param name="onValueChanged">Callback to invoke when value at the specified path changed.</param>
        /// <exception cref="ArgumentException">Thrown if path is invalid for this tree.</exception>
        /// <returns>Current value at specified path.</returns>
        public object RegisterListener(string path, Action<object> onValueChanged)
        {
            var node = this.ResolvePath(path);

            // Register for value change.
            node.ValueChanged += onValueChanged;

            return node.Value;
        }

        /// <summary>
        ///     Removes the callback from the specified path.
        /// </summary>
        /// <param name="path">Path to remove callback from.</param>
        /// <param name="onValueChanged">Callback to remove.</param>
        /// <exception cref="ArgumentException">Thrown if path is invalid for this tree.</exception>
        public void RemoveListener(string path, Action<object> onValueChanged)
        {
            var node = this.ResolvePath(path);

            // Remove from value change.
            node.ValueChanged -= onValueChanged;
        }

        /// <summary>
        ///     Sets the specified value at the specified path.
        /// </summary>
        /// <param name="path">Path to set the data value at.</param>
        /// <exception cref="ArgumentException">Thrown if path is invalid for this tree.</exception>
        /// <exception cref="InvalidOperationException">Thrown if data at specified path can't be changed.</exception>
        /// <param name="value">Value to set.</param>
        public void SetValue(string path, object value)
        {
            var node = this.ResolvePath(path);
            node.SetValue(value);
        }

        /// <summary>
        ///     Resolves a path starting from the root node and returns the found node.
        /// </summary>
        /// <param name="path">Path to resolve.</param>
        /// <returns>Node found at the specified path.</returns>
        /// <exception cref="ArgumentException">Thrown if path is invalid for this tree.</exception>
        private IDataNode ResolvePath(string path)
        {
            var node = this.root.FindDescendant(path);
            if (node == null)
            {
                throw new ArgumentException("Invalid path '" + path + "' for type " + this.root.DataType, "path");
            }
            return node;
        }
    }
}