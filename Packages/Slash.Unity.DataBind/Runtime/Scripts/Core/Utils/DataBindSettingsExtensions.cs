// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBindSettingsExtensions.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System;

    /// <summary>
    ///   Extension methods for the <see cref="IDataBindSettings"/> interface.
    /// </summary>
    public static class DataBindSettingsExtensions
    {
        /// <summary>
        ///     Returns the name of the data provider for the property with the specified name.
        /// </summary>
        /// <param name="settings">Data Bind settings.</param>
        /// <param name="name">Name of property to get data provider name for.</param>
        /// <returns>Name of the data provider for the property with the specified name.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid data provider format is set in the settings.</exception>
        public static string GetDataProviderName(this IDataBindSettings settings, string name)
        {
            string dataProviderName;
            switch (settings.DataProviderFormatSetting)
            {
                case DataBindSettings.DataProviderFormats.FirstLetterLowerCase:
                    dataProviderName = char.ToLowerInvariant(name[0]) + name.Substring(1);
                    break;
                case DataBindSettings.DataProviderFormats.FirstLetterUpperCase:
                    dataProviderName = name;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (settings.AppendUnderscoreSetting)
            {
                dataProviderName = "_" + dataProviderName;
            }

            return dataProviderName + "Property";
        }
    }
}