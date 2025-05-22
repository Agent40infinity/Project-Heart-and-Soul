namespace Slash.Unity.DataBind.Addons.TextMeshPro.Setters
{
    using Slash.Unity.DataBind.Foundation.Setters;

    using TMPro;

    using UnityEngine;

    /// <summary>
    ///   Sets the text of a TextMeshPro text component.
    /// </summary>
    [AddComponentMenu("Data Bind/TextMeshPro/Setters/[DB] TextMeshPro Text Setter")]
    public class TextMeshProTextSetter : ComponentSingleSetter<TMP_Text, string>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(TMP_Text target, string value)
        {
            target.SetText(value ?? string.Empty);
        }
    }
}