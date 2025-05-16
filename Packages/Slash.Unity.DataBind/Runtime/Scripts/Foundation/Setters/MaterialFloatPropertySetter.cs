using UnityEngine;

namespace Slash.Unity.DataBind.Foundation.Setters
{
    /// <summary>
    ///    Setter for a float property of a material.
    /// </summary>
    public class MaterialFloatPropertySetter : SingleSetter<float>
    {
        /// <summary>
        ///   Material to set property for.
        /// </summary>
        public Material Material;

        /// <summary>
        ///   Name of property to set.
        /// </summary>
        public string PropertyName;

        /// <summary>
        ///   Called when the data binding value changed.
        /// </summary>
        /// <param name="newValue">New data value.</param>
        protected override void OnValueChanged(float newValue)
        {
            if (this.Material != null)
            {
                this.Material.SetFloat(this.PropertyName, newValue);
            }
        }
    }
}