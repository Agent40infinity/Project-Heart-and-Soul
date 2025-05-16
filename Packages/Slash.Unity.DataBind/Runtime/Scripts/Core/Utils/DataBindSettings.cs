// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBindSettings.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System.IO;
    using System.Linq;
    using Slash.Unity.DataBind.Core.Data;
    using UnityEngine;
    using UnityEngine.Serialization;
#if UNITY_EDITOR
    using UnityEditor;

#endif

    /// <summary>
    ///     Settings that can be set for the specific project Data Bind is used in.
    /// </summary>
    public class DataBindSettings : ScriptableObject, IDataBindSettings
    {
        /// <summary>
        ///     Available naming formats for data property fields in <see cref="IDataContext" /> classes.
        /// </summary>
        public enum DataProviderFormats
        {
            /// <summary>
            ///     The data provider name is the same as the data name, but with the first letter being lower case
            ///     and a "Property" postfix.
            /// </summary>
            FirstLetterLowerCase,

            /// <summary>
            ///     The data provider name is the same as the data name,
            ///     but only with the "Property" postfix.
            /// </summary>
            FirstLetterUpperCase
        }

        /// <summary>
        ///     Separator in context paths.
        /// </summary>
        public const char PathSeparator = '.';

        /// <summary>
        ///     Path to use to get context itself instead of a data property from the context.
        /// </summary>
        public const string SelfReferencePath = "#";

        private const string SettingsPath = "Data Bind Settings";

        private const string DefaultSettingsAssetPath =
            "Assets/Slash.Unity.DataBind/Resources/" + SettingsPath + ".asset";

        private static DataBindSettings instance;

        /// <summary>
        ///     Indicates if an underscore should be appended to form the data provider field/property name.
        /// </summary>
        public bool AppendUnderscoreSetting
        {
            get
            {
                return this.AppendUnderscore;
            }
        }

        /// <summary>
        ///     Naming format to use for the data property fields in a <see cref="IDataContext" /> class.
        /// </summary>
        public DataProviderFormats DataProviderFormatSetting
        {
            get
            {
                return this.DataProviderFormat;
            }
        }

        /// <inheritdoc />
        public bool DataProviderIsCaseInsensitive
        {
            get
            {
                return this.dataProviderIsCaseInsensitive; 
            }
        }

        /// <summary>
        ///     Singleton instance of settings.
        ///     Will create the settings file when executed in editor.
        /// </summary>
        public static DataBindSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Resources.Load<DataBindSettings>(SettingsPath);
                    if (instance == null)
                    {
                        // Use any DataBindSettings in project.
                        var settingsObjects = Resources.FindObjectsOfTypeAll<DataBindSettings>();
                        if (settingsObjects.Length > 0)
                        {
                            instance = settingsObjects.First();
                            if (settingsObjects.Length > 1)
                            {
                                Debug.LogWarningFormat(instance,
                                    "Found {0} data bind settings resources. Name the one to use '{1}', otherwise the wrong one might be chosen (using: '{2}')",
                                    settingsObjects.Length, SettingsPath, instance.name);
                            }
                        }
                    }
#if UNITY_EDITOR
                    if (instance == null)
                    {
                        instance = AssetDatabase.LoadAssetAtPath<DataBindSettings>(DefaultSettingsAssetPath);
                    }
#endif
                    if (instance == null)
                    {
                        instance = CreateInstance<DataBindSettings>();
#if UNITY_EDITOR
                        var settingsDirectoryName = Path.GetDirectoryName(DefaultSettingsAssetPath);
                        if (!string.IsNullOrEmpty(settingsDirectoryName))
                        {
                            Directory.CreateDirectory(settingsDirectoryName);
                        }
                        AssetDatabase.CreateAsset(instance, DefaultSettingsAssetPath);
                        AssetDatabase.SaveAssets();
#endif
                    }
                }
                return instance;
            }
        }

        /// <inheritdoc />
        public bool ReportMissingBackingFields
        {
            get
            {
#if DATABIND_REPORT_MISSING_BACKING_FIELDS
                return true;
#else
                return false;
#endif
            }
        }

        /// <inheritdoc />
        public void LogWarning(string log)
        {
            Debug.LogWarning(log);
        }

        #region Data Provider Format

        /// <summary>
        ///     Indicates if an underscore should be appended to form the data provider field/property name.
        /// </summary>
        [Tooltip("Indicates if an underscore should be appended to form the data provider field/property name")]
        [FormerlySerializedAs("AppendUnderscore")]
        public bool AppendUnderscore;

        /// <summary>
        ///     Naming format to use for the data property fields in a <see cref="IDataContext" /> class.
        /// </summary>
        [Header("Data Provider Format")]
        [Tooltip("Indicates how the data provider field/property name is created from the data name")]
        [FormerlySerializedAs("DataProviderFormat")]
        public DataProviderFormats DataProviderFormat;

        /// <summary>
        ///   Indicates that the lookup of data property fields is case insensitive.
        /// </summary>
        [Tooltip("Indicates that the lookup of data property fields is case insensitive")]
        [SerializeField]
        private bool dataProviderIsCaseInsensitive = false;

        #endregion
    }
}