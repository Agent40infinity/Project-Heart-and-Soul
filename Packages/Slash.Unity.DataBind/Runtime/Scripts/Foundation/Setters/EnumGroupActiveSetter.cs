namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Collections.Generic;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    public abstract class EnumGroupActiveSetter<TEnum> : DataBindingOperator
    {
        /// <summary>
        ///     Provider for enum value.
        /// </summary>
        public DataBinding EnumValue;

        private GroupActiveSetter<TEnum> groupActiveSetter;

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            if (this.groupActiveSetter != null)
            {
                this.groupActiveSetter.Disable();
                this.groupActiveSetter = null;
            }
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            this.groupActiveSetter = new GroupActiveSetter<TEnum>(
                this.GetGameObjects(), this.EnumValue,
                this.IsGameObjectActive);
            this.groupActiveSetter.Enable();
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();

            this.AddBinding(this.EnumValue);
        }

        /// <summary>
        ///     Returns the game object which is mapped to the specified enum value.
        /// </summary>
        /// <param name="enumValue">Enum value to get game object for.</param>
        /// <returns>Game object which is mapped to the specified enum value; null if none is mapped.</returns>
        protected abstract GameObject GetGameObject(TEnum enumValue);

        /// <summary>
        ///     Returns all game objects of the group.
        /// </summary>
        /// <returns>All game objects of the group.</returns>
        protected abstract IEnumerable<GameObject> GetGameObjects();

        private bool IsGameObjectActive(GameObject gameObjectValue, TEnum enumValue)
        {
            var gameObjectForEnumValue = this.GetGameObject(enumValue);
            return gameObjectForEnumValue == gameObjectValue;
        }
    }
}