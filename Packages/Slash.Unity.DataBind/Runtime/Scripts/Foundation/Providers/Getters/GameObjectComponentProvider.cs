namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Provider to get a specific component from a game object.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to provide.</typeparam>
    public class GameObjectComponentProvider<TComponent> : DataProvider
        where TComponent : Component
    {
        /// <summary>
        ///   Binding to target game object to get component from.
        /// </summary>
        public DataBinding Target;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var targetGameObject = this.Target.GetValue<GameObject>();
                return targetGameObject != null ? targetGameObject.GetComponent<TComponent>() : null;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();
            this.RemoveBinding(this.Target);
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();
            this.AddBinding(this.Target);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}