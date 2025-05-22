// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumGetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using System;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;

    /// <summary>
    ///     Provides the enum values of a specified enum type.
    /// </summary>
    public class EnumGetter : DataProvider
    {
        [SerializeField]
        [TypeSelection(BaseType = typeof(Enum))]
        private string enumType;

        /// <summary>
        ///     Type of enum to get.
        /// </summary>
        public Type EnumType
        {
            get
            {
                try
                {
                    return this.enumType != null ? ReflectionUtils.FindType(this.enumType) : null;
                }
                catch (TypeLoadException)
                {
                    Debug.LogError("Can't find type '" + this.enumType + "'.", this);
                    return null;
                }
            }
            set
            {
                this.enumType = value != null ? value.AssemblyQualifiedName : null;
            }
        }

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                if (this.EnumType == null)
                {
                    return null;
                }
                return Enum.GetValues(this.EnumType);
            }
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            // Initial value change.
            this.OnValueChanged();
        }
    }
}