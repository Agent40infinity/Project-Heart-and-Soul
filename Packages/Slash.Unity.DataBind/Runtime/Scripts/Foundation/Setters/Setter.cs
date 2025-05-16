// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Setter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Collections.Generic;

    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Base class of a setter which influences the presentation depending on one or more data bindings.
    /// </summary>
    public abstract class Setter : MonoBehaviour
    {
        #region Fields

        private readonly List<DataBinding> bindings = new List<DataBinding>();

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public void OnContextChanged()
        {
            foreach (var binding in this.bindings)
            {
                binding.OnContextChanged();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Adds and initializes the specified binding which the setter depends on.
        /// </summary>
        /// <param name="binding">Binding to add.</param>
        protected void AddBinding(DataBinding binding)
        {
            // Init.
            binding.Init(DataBindRunner.Instance.DataContextNodeConnectorInitializer, this);

            this.bindings.Add(binding);
        }

        /// <summary>
        ///   Removes and deinitializes the specified binding.
        /// </summary>
        /// <param name="binding">Binding to remove.</param>
        protected void RemoveBinding(DataBinding binding)
        {
            // Deinit.
            binding.Deinit();

            this.bindings.Remove(binding);
        }

        #endregion
    }
}