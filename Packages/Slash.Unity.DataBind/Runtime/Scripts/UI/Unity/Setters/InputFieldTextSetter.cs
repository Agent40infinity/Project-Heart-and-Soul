// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InputFieldTextSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Set the text of a InputField depending on the string data value.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Setters/[DB] Input Field Text Setter (Unity)")]
    public class InputFieldTextSetter : ComponentSingleSetter<InputField, string>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(InputField target, string value)
        {
            target.text = value ?? string.Empty;
        }
    }
}