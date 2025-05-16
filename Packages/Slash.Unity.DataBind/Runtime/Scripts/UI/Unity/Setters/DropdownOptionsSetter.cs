// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DropdownOptionsSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using System.Collections;
    using System.Linq;

    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///   Set the options of a dropdown element to the provided enumeration of option data items.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Dropdown Options Setter (Unity)")]
    public class DropdownOptionsSetter : ComponentSingleSetter<Dropdown, IEnumerable>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(Dropdown target, IEnumerable value)
        {
            target.ClearOptions();
            if (value != null)
            {
                target.AddOptions(value.OfType<Dropdown.OptionData>().ToList());
            }
        }
    }
}