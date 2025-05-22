namespace Slash.Unity.DataBind.Addons.TextMeshPro.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;
    using TMPro;
    using UnityEngine;

    /// <summary>
    ///     Sets the text of a TextMeshPro input field.
    /// </summary>
    [AddComponentMenu("Data Bind/TextMeshPro/Setters/[DB] TextMeshPro InputField Text Setter")]
    public class TextMeshProInputFieldTextSetter : ComponentSingleSetter<TMP_InputField, string>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(TMP_InputField target, string value)
        {
            target.text = value ?? string.Empty;
        }
    }
}