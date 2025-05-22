namespace Slash.Unity.DataBind.Core.Utils
{
    using Slash.Unity.DataBind.Core.Data;

    /// <summary>
    ///     Interface for the settings.
    /// </summary>
    public interface IDataBindSettings
    {
        /// <summary>
        ///     Indicates if an underscore should be appended to form the data provider field/property name.
        /// </summary>
        bool AppendUnderscoreSetting { get; }

        /// <summary>
        ///     Naming format to use for the data property fields in a <see cref="IDataContext" /> class.
        /// </summary>
        DataBindSettings.DataProviderFormats DataProviderFormatSetting { get; }
        
        /// <summary>
        ///   Indicates that the lookup of data property fields is case insensitive.
        /// </summary>
        bool DataProviderIsCaseInsensitive { get; }

        /// <summary>
        ///     Indicates if a warning should be logged when a context path doesn't have a backing field.
        /// </summary>
        bool ReportMissingBackingFields { get; }

        /// <summary>
        ///    Logs a warning.
        /// </summary>
        /// <param name="log">Text to log.</param>
        void LogWarning(string log);
    }
}