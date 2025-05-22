// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameObjectTransformProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Provides the transform of the target game object.
    /// </summary>
    public class GameObjectTransformProvider : DataProvider
    {
        /// <summary>
        ///     Game object to get transform from.
        /// </summary>
        public DataBinding Target;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var targetGameObject = this.Target.GetValue<GameObject>();
                return targetGameObject != null ? targetGameObject.transform : null;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Target);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Target);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}