// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameObjectActiveSwitch.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Switch which activates/deactivates two game objects depending on the boolean data value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] GameObject Active Switch")]
    public class GameObjectActiveSwitch : DataBindingOperator
    {
        /// <summary>
        ///     Game object to activate when switch is false.
        /// </summary>
        [Tooltip("Game object to activate when switch is false")]
        [DataTypeHintExplicit(typeof(GameObject))]
        public DataBinding ActiveWhenFalse;

        /// <summary>
        ///     Game object to activate when switch is true.
        /// </summary>
        [Tooltip("Game object to activate when switch is true")]
        [DataTypeHintExplicit(typeof(GameObject))]
        public DataBinding ActiveWhenTrue;

        /// <summary>
        ///     Value that decides which game object to activate.
        /// </summary>
        [Tooltip("Value that decides which game object to activate")]
        [DataTypeHintExplicit(typeof(bool))]
        public DataBinding Switch;

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.Switch);
            this.RemoveBinding(this.ActiveWhenTrue);
            this.RemoveBinding(this.ActiveWhenFalse);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.Switch);
            this.AddBinding(this.ActiveWhenTrue);
            this.AddBinding(this.ActiveWhenFalse);
        }

        /// <inheritdoc />
        protected override void OnBindingValuesChanged()
        {
            var switchValue = this.Switch.GetValue<bool>();

            var activeWhenTrueGameObject = this.ActiveWhenTrue.GetValue<GameObject>();
            if (activeWhenTrueGameObject != null)
            {
                activeWhenTrueGameObject.SetActive(switchValue);
            }

            var activeWhenFalseGameObject = this.ActiveWhenFalse.GetValue<GameObject>();
            if (activeWhenFalseGameObject != null)
            {
                activeWhenFalseGameObject.SetActive(!switchValue);
            }
        }
    }
}