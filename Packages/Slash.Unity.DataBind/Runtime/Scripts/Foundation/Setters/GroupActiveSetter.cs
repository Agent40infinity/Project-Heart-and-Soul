// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GroupActiveSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System;
    using System.Collections.Generic;
    using Slash.Unity.DataBind.Core.Data;
    using UnityEngine;

    /// <summary>
    ///     Sets a group of game objects active/inactive depending on a data value and a check function.
    /// </summary>
    /// <typeparam name="TData">Type of data.</typeparam>
    public class GroupActiveSetter<TData>
    {
        private readonly IDataProvider dataValueProvider;

        private readonly IEnumerable<GameObject> gameObjects;

        private readonly Func<GameObject, TData, bool> isActiveCheck;

        /// <inheritdoc />
        public GroupActiveSetter(IEnumerable<GameObject> gameObjects,
            IDataProvider dataValueProvider,
            Func<GameObject, TData, bool> isActiveCheck)
        {
            this.dataValueProvider = dataValueProvider;
            this.gameObjects = gameObjects;
            this.isActiveCheck = isActiveCheck;
        }

        public void Disable()
        {
            this.dataValueProvider.ValueChanged -= this.OnDataValueChanged;
        }

        public void Enable()
        {
            this.dataValueProvider.ValueChanged += this.OnDataValueChanged;
            this.UpdateGameObjects();
        }

        private void OnDataValueChanged()
        {
            this.UpdateGameObjects();
        }

        private void UpdateGameObjects()
        {
            // Update game objects.
            var enumValue = this.dataValueProvider.Value is TData
                ? (TData) this.dataValueProvider.Value
                : default(TData);
            foreach (var gameObject in this.gameObjects)
            {
                gameObject.SetActive(this.isActiveCheck(gameObject, enumValue));
            }
        }
    }
}