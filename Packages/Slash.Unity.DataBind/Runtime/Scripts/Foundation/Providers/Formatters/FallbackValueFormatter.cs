// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FallbackValueFormatter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Provides a fallback value if the specified data value is not set.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Fallback Value Formatter")]
    public class FallbackValueFormatter : DataProvider
    {
        /// <summary>
        ///     Binding to get data from.
        /// </summary>
        public DataBinding Data;

        /// <summary>
        ///     Binding to get data value from if other binding doesn't provide a value.
        /// </summary>
        public DataBinding Fallback;

        /// <summary>
        ///     Current data value.
        /// </summary>
        public override object Value
        {
            get
            {
                return this.Data.Value ?? this.Fallback.Value;
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Data);
            this.AddBinding(this.Fallback);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}