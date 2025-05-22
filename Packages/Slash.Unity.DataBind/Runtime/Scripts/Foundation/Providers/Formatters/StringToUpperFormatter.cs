// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringToUpperFormatter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Formats a string by converting all letters to upper-case.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] String ToUpper Formatter")]
    public class StringToUpperFormatter : DataProvider
    {
        /// <summary>
        ///     String to convert.
        /// </summary>
        public DataBinding Argument;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                if (this.Argument == null)
                {
                    return string.Empty;
                }

                var argument = this.Argument.GetValue<string>();
                return string.IsNullOrEmpty(argument) ? string.Empty : argument.ToUpper();
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            // Add bindings.
            this.AddBinding(this.Argument);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            // TODO(co): Cache current value and check if really changed?
            this.OnValueChanged();
        }
    }
}