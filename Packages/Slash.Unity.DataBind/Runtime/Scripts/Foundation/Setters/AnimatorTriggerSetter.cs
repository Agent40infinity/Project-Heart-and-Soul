// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AnimatorTriggerSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using UnityEngine;

    /// <summary>
    ///   Sets the animator paramater of a game object to the boolean data value.
    ///   <para>Input: Boolean</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Animator Trigger Setter")]
    public class AnimatorTriggerSetter : ComponentSingleSetter<Animator, bool>
    {
        /// <summary>
        ///   Name of the animator parameter.
        /// </summary>
        [Tooltip("Name of an animator parameter.")]
        public string AnimatorParameterName;

        /// <inheritdoc />
        protected override void UpdateTargetValue(Animator target, bool value)
        {
            if (!target.isInitialized)
            {
                // Ignore trigger if animator is not ready yet.
                return;
            }

            if (value)
            {
                target.SetTrigger(this.AnimatorParameterName);
            }
            else
            {
                target.ResetTrigger(this.AnimatorParameterName);
            }
            
        }
    }
}