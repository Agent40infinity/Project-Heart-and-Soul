namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using UnityEngine;

    /// <summary>
    ///   Base class for a component data provider which couldn't use an event to determine
    ///   when its data value changed, but has to do it in the frame update.
    /// </summary>
    public abstract class ComponentPullDataProvider<TComponent, TData> : ComponentDataProvider<TComponent, TData>
        where TComponent : Component
    {
        /// <summary>
        ///   Current data value.
        /// </summary>
        private TData currentValue;

        /// <summary>
        ///   Per frame update.
        /// </summary>
        public void Update()
        {
            var target = this.Target;
            if (target == null)
            {
                return;
            }

            var newRotation = this.GetValue(target);
            if (!Equals(newRotation, this.currentValue))
            {
                this.currentValue = newRotation;
                this.OnValueChanged();
            }
        }

        /// <inheritdoc />
        protected override void AddListener(TComponent target)
        {
            // There is no event to listen to.
        }

        /// <inheritdoc />
        protected override void RemoveListener(TComponent target)
        {
            // There is no event to listen to.
        }
    }
}