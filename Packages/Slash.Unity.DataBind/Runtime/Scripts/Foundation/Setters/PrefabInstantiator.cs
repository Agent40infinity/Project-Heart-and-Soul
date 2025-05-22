// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrefabInstantiator.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Instantiates a game object from the bound prefab if one is provided.
    /// </summary>
    public class PrefabInstantiator : DataBindingOperator
    {
        /// <summary>
        ///   Parent transform to add instantiated game object to.
        /// </summary>
        public DataBinding Parent;

        /// <summary>
        ///   Prefab to instantiate.
        /// </summary>
        public DataBinding Prefab;

        private GameObject prefabGameObject;

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();
            this.Prefab.ValueChanged -= this.OnPrefabChanged;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();
            this.Prefab.ValueChanged += this.OnPrefabChanged;
            this.UpdateGameObject();
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Prefab);
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Prefab);
        }

        private void CreateGameObject()
        {
            var prefab = this.Prefab.GetValue<GameObject>();
            if (prefab == null)
            {
                return;
            }

            var parent = this.Parent.GetValue<Transform>() ?? this.transform;
#if UNITY_5_5_OR_NEWER
            this.prefabGameObject = Instantiate(prefab, parent, false);
			#else
            this.prefabGameObject = Instantiate(prefab);
            this.prefabGameObject.transform.SetParent(parent);
#endif
            this.prefabGameObject.transform.localPosition = Vector3.zero;
        }

        private void OnPrefabChanged()
        {
            this.UpdateGameObject();
        }

        private void RemoveGameObject()
        {
            if (this.prefabGameObject != null)
            {
                Destroy(this.prefabGameObject);
                this.prefabGameObject = null;
            }
        }

        private void UpdateGameObject()
        {
            this.RemoveGameObject();
            this.CreateGameObject();
        }
    }
}