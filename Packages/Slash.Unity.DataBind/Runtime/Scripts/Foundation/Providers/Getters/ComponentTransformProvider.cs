using UnityEngine;

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    /// <summary>
    ///   Returns the transform of a specific component.
    /// </summary>
    public class ComponentTransformProvider : ComponentDataProvider<Component, Transform>
    {
        /// <summary>
        ///     Register listener at target to be informed if its value changed.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to add listener to.</param>
        protected override void AddListener(Component target)
        {
        }

        /// <summary>
        ///     Derived classes should return the current value to set if this method is called.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to get value from.</param>
        /// <returns>Current value to set.</returns>
        protected override Transform GetValue(Component target)
        {
            return target.transform;
        }

        /// <summary>
        ///     Remove listener from target which was previously added in AddListener.
        ///     The target is already checked for null reference.
        /// </summary>
        /// <param name="target">Target to remove listener from.</param>
        protected override void RemoveListener(Component target)
        {
        }
    }
}