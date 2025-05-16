// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectableInteractableSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Sets if a selectable is interactable depending on a boolean data value.
    ///     <para>Input: Boolean</para>
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Selectable Interactable Setter (Unity)")]
    public class SelectableInteractableSetter : ComponentSingleSetter<Selectable, bool>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Selectable target, bool value)
        {
            target.interactable = value;
        }
    }
}