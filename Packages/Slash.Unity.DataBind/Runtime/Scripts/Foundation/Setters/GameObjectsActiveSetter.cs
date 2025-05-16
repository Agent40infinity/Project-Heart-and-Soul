namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    ///     Enables/disables a collection of game objects depending on a data value
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Game Objects Active Setter")]
    public class GameObjectsActiveSetter : SingleSetter<bool>
    {
        /// <summary>
        ///     Game objects to enable/disable
        /// </summary>
        [Tooltip("Game objects to enable/disable")]
        public List<GameObject> GameObjects;

        private GroupActiveSetter<bool> groupActiveSetter;

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

            this.groupActiveSetter = new GroupActiveSetter<bool>(
                this.GameObjects, this.Data,
                (gameObjectValue, isActive) => isActive);
            this.groupActiveSetter.Enable();
        }

        /// <inheritdoc />
        protected override void OnValueChanged(bool newValue)
        {
        }
    }
}