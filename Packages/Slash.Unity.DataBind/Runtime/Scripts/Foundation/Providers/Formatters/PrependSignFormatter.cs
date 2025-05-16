// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PrependSignFormatter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Prepends in front of the data value.
    ///     <para>Input: Number</para>
    ///     <para>Output: String</para>
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] Prepend Sign")]
    public class PrependSignFormatter : DataProvider
    {
        /// <summary>
        ///     Data value to use.
        /// </summary>
        public DataBinding Data;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var argumentA = this.Data.GetValue<float>();
                return string.Format("{0}{1}", argumentA > 0 ? "+" : string.Empty, argumentA);
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            // Add bindings.
            this.AddBinding(this.Data);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}