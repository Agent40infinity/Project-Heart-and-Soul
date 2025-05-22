// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnityUtils.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    using UnityEngine;

    /// <summary>
    ///   Utility methods for Unity game objects.
    /// </summary>
    public static class UnityUtils
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Instantiates a new game object from the specified prefab and adds it
        ///   to the game object.
        ///   Makes sure the position/rotation/scale are initialized correctly.
        /// </summary>
        /// <param name="parent">Game object to add child to.</param>
        /// <param name="prefab">Prefab to instantiate new child from.</param>
        /// <param name="resetScale">If the scale of the child is reset.</param>
        /// <returns>Instantiated new child.</returns>
        public static GameObject AddChild(this GameObject parent, GameObject prefab, bool resetScale = true)
        {
            var go = prefab != null ? Object.Instantiate(prefab) as GameObject : new GameObject();
            if (go == null || parent == null)
            {
                return go;
            }

            var t = go.transform;
            t.SetParent(parent.transform, false);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            if (resetScale)
            {
                t.localScale = Vector3.one;
            }
            go.layer = parent.layer;

            return go;
        }

        /// <summary>
        ///   Destroys all children from the specified game object.
        /// </summary>
        /// <param name="gameObject">Game object to destroy children.</param>
        public static void DestroyChildren(this GameObject gameObject)
        {
            var children = gameObject.GetChildren().ToList();
            foreach (var child in children)
            {
                // Set inactive to hide immediatly. The destruction is just performed after the next update.
                child.SetActive(false);
                Object.Destroy(child);
            }
        }

        /// <summary>
        ///   Collects all children from the specified game object.
        /// </summary>
        /// <param name="gameObject">Game object to collect children from.</param>
        /// <returns>Enumeration of all children of the specified game object.</returns>
        public static IEnumerable<GameObject> GetChildren(this GameObject gameObject)
        {
            return (from Transform child in gameObject.transform select child.gameObject);
        }

        #endregion
    }
}