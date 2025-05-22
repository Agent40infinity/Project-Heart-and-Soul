// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MasterPath.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;

    /// <summary>
    ///   A path to prepend when searching for a data context property.
    /// </summary>
    [AddComponentMenu("Data Bind/Core/[DB] Master Path")]
    public class MasterPath : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Path to prepend.
        /// </summary>
        [ContextPath(Filter = ContextMemberFilter.Contexts | ContextMemberFilter.Recursive)]
        public string Path = string.Empty;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Returns the full path, i.e. this path and all ancestor paths prepended.
        /// </summary>
        /// <returns>Full path.</returns>
        public string GetFullPath()
        {
            var parentMasterPath = GetFullPathUpwards(this.transform.parent);
            var path = this.Path;
            var fullPath = (string.IsNullOrEmpty(parentMasterPath)) ? path : (parentMasterPath + "." + path);
            return fullPath;
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Searches upwards in the hierarchy for the first master path and returns
        ///   its full path.
        /// </summary>
        /// <param name="obj">Transform to start search at.</param>
        /// <returns>Full path of first found master path.</returns>
        private static string GetFullPathUpwards(Transform obj)
        {
            if (obj == null)
            {
                return null;
            }
            var masterPath = obj.GetComponent<MasterPath>();
            if (masterPath != null)
            {
                return masterPath.GetFullPath();
            }
            return obj.transform.parent != null ? GetFullPathUpwards(obj.parent) : null;
        }

        #endregion
    }
}