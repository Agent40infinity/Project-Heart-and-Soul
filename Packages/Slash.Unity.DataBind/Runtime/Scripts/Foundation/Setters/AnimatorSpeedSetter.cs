namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System.Collections;

    using UnityEngine;

    /// <summary>
    ///   Sets the speed of the animator to the float data value.
    ///   <para>Input: Boolean</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Animator Speed Setter")]
    public class AnimatorSpeedSetter : ComponentSingleSetter<Animator, float>
    {
        /// <summary>
        ///   Coroutine which will set the initial value.
        /// </summary>
        private Coroutine initializerCoroutine;

        /// <inheritdoc />
        protected override void UpdateTargetValue(Animator target, float value)
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
                this.initializerCoroutine = this.StartCoroutine((IEnumerator)this.InitializeAnimatorParameter(target, value));
            }
        }

        private void SetAnimatorParameter(Animator target, float newValue)
        {
            target.speed = newValue;
        }

        private IEnumerator InitializeAnimatorParameter(Animator target, float value)
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