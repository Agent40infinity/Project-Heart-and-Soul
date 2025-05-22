// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBindingSettingsProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System;
    using System.Reflection;
    using Slash.Unity.DataBind.Core.Data;

    /// <summary>
    ///     Provides the data binding settings to use.
    /// </summary>
    public static class DataBindingSettingsProvider
    {
        /// <summary>
        ///     Current settings to use.
        /// </summary>
        public static IDataBindSettings Settings
        {
            get
            {
                if (SettingsOverride != null)
                {
                    return SettingsOverride;
                }

                return DataBindSettings.Instance;
            }
        }

        /// <summary>
        ///     Settings to use instead of the <see cref="DataBindSettings" /> instance.
        /// </summary>
        public static IDataBindSettings SettingsOverride { get; set; }

        /// <summary>
        ///     Returns the data provider of the specified object and type with the specified name.
        /// </summary>
        /// <param name="obj">Concrete object to get data provider for.</param>
        /// <param name="name">Name of data provider to get.</param>
        /// <returns>Data provider for the specified object and name.</returns>
        public static IDataProvider GetDataProvider(object obj, string name)
        {
            if (obj == null || string.IsNullOrEmpty(name))
            {
                return null;
            }

            // Go up the type hierarchy of the object.
            // NOTE(co): Private fields of base classes are not reflected for derived types.
            var type = obj.GetType();
            while (type != null)
            {
                var property = GetDataProvider(type, obj, name);
                if (property != null)
                {
                    return property;
                }

                type = TypeInfoUtils.GetBaseType(type);
            }

            return null;
        }

        /// <summary>
        ///     Returns the data provider of the specified object and type with the specified name.
        /// </summary>
        /// <param name="type">Type to search.</param>
        /// <param name="obj">Concrete object to get data provider for.</param>
        /// <param name="name">Name of data to get data provider for.</param>
        /// <returns>Data provider for the specified object and name, using the specified type for reflection.</returns>
        private static IDataProvider GetDataProvider(Type type, object obj, string name)
        {
            var settings = Settings;

            var dataProviderName = settings.GetDataProviderName(name);

            var additionalFlags = BindingFlags.Default;
            if (settings.DataProviderIsCaseInsensitive)
            {
                additionalFlags |= BindingFlags.IgnoreCase;
            }

            // Check for field.
            var dataProviderField = TypeInfoUtils.GetPrivateField(type, dataProviderName, additionalFlags);
            if (dataProviderField != null)
            {
                return dataProviderField.GetValue(obj) as IDataProvider;
            }

            // Check for public property.
            var dataProviderProperty = TypeInfoUtils.GetPublicProperty(type, dataProviderName, additionalFlags);
            if (dataProviderProperty != null)
            {
                return dataProviderProperty.GetValue(obj, null) as IDataProvider;
            }

			// Is this a public provider? (LocalizedString is this)
			var publicDataProviderField = TypeInfoUtils.GetPublicField(type, name);
			if (publicDataProviderField != null)
			{
				return publicDataProviderField.GetValue(obj) as IDataProvider;
			}

            return null;
        }
    }
}