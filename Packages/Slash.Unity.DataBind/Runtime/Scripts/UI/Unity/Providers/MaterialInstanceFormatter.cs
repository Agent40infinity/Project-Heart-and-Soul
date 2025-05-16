// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialInstanceFormatter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Providers
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Creates a new material instance from the specified material.
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Formatters/[DB] Material Instance Formatter")]
    public class MaterialInstanceFormatter : DataProvider
    {
        /// <summary>
        ///     Material to instantiate.
        /// </summary>
        public DataBinding Material;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var material = this.Material.GetValue<Material>();
                return material != null ? new Material(material) : null;
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            // Add bindings.
            this.AddBinding(this.Material);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}