// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SelectableInteractableProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.UI.Unity.Providers
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    ///     Provides the interactable flag of a <see cref="Selectable" />
    /// </summary>
    [AddComponentMenu("Data Bind/UnityUI/Providers/[DB] Selectable Interactable Provider")]
    public class SelectableInteractableProvider : DataProvider
    {
        /// <summary>
        ///     Selectable to get flag for
        /// </summary>
        [DataTypeHintExplicit(typeof(Selectable))]
        public DataBinding Selectable;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var selectable = this.Selectable.GetValue<Selectable>();
                return selectable != null && selectable.interactable;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Selectable);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Selectable);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}