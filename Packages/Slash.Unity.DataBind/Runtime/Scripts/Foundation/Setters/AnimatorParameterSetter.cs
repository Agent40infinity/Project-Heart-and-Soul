namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    ///     Base class for a setter that updates a parameter of an <see cref="Animator" />.
    /// </summary>
    /// <typeparam name="T">Type of parameter this setter handles.</typeparam>
    public abstract class AnimatorParameterSetter<T> : ComponentSingleSetter<Animator, T>
    {
        /// <summary>
        ///     Name of the animator parameter.
        /// </summary>
        [Tooltip("Name of an animator parameter.")]
        public string AnimatorParameterName;

        /// <summary>
        ///     Coroutine which will set the initial value.
        /// </summary>
        private Coroutine initializerCoroutine;

        /// <inheritdoc />
        protected override void UpdateTargetValue(Animator target, T value)
        {
            // Stop previous initializer.
            if (this.initializerCoroutine != null)
            {
                this.StopCoroutine(this.initializerCoroutine);
                this.initializerCoroutine = null;
            }

            if (target.isInitialized)
            {
                this.SetAnimatorParameter(target, value);
            }
            else
            {
                // Delay setting parameter.
                this.initializerCoroutine = this.StartCoroutine(this.InitializeAnimatorParameter(target, value));
            }
        }

        /// <summary>
        ///     Called when the animator parameter should be set to the specified value.
        /// </summary>
        /// <param name="target">Animator to update.</param>
        /// <param name="newValue">Value to set animator parameter to.</param>
        protected abstract void SetAnimatorParameter(Animator target, T newValue);

        private IEnumerator InitializeAnimatorParameter(Animator target, T value)
        {
            while (!target.isInitialized)
            {
                yield return new WaitForEndOfFrame();
            }

            this.SetAnimatorParameter(target, value);

            this.initializerCoroutine = null;
        }
    }
}