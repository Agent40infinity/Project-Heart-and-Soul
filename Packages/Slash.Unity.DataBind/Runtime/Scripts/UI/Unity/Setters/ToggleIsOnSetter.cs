// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToggleIsOnSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Enables/Disables a toggle control depending on a boolean data value.
    ///   <para>
    ///     Input: <see cref="bool" />
    ///   </para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Toggle IsOn Setter (Unity)")]
    public class ToggleIsOnSetter : ComponentSingleSetter<Toggle, bool>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Toggle target, bool value)
        {
            target.isOn = value;
        }
    }
}