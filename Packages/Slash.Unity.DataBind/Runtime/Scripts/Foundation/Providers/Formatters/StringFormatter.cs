// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringFormatter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Formatters
{
    using System;
    using System.Linq;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Formats arguments by a specified format string to create a new string value.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Formatters/[DB] String Formatter")]
    [DataTypeHintExplicit(typeof(string))]
    public class StringFormatter : DataProvider
    {
        /// <summary>
        ///     Arguments to put into the string.
        /// </summary>
        public DataBinding[] Arguments;

        /// <summary>
        ///     Format to use.
        /// </summary>
        public DataBinding Format;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var format = string.Empty;
                if (this.Format != null)
                {
                    format = this.Format.GetValue<string>();
                }

                var texts = this.Arguments != null ? this.Arguments.Select(argument => argument.Value).ToArray() : null;
                string newValue = null;
                try
                {
                    if (!string.IsNullOrEmpty(format))
                    {
                        // Convert special characters.
                        format = format.Replace("\\n", "\n");
                        
                        newValue = texts != null
                            ? string.Format(format, texts)
                            : format;
                    }
                    else
                    {
                        newValue = string.Empty;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Exception formatting value: " + e);
                    return format;
                }

                return newValue;
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            // Add bindings.
            this.AddBinding(this.Format);
            this.AddBindings(this.Arguments);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            // TODO(co): Cache current value and check if really changed?
            this.OnValueChanged();
        }
    }
}