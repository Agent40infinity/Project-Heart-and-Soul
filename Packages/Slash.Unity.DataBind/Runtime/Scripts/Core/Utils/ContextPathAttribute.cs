// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextPathAttribute.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using UnityEngine;

    /// <summary>
    ///   Use this attribute on a path field which should be visualized
    ///   with the PathPropertyDrawer.
    /// </summary>
    public class ContextPathAttribute : PropertyAttribute
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Constructor.
        /// </summary>
        public ContextPathAttribute()
        {
            this.Filter = ContextMemberFilter.All;
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Determines which members of the context should be selectable.
        /// </summary>
        public ContextMemberFilter Filter { get; set; }

        /// <summary>
        ///   Name to show next to the selected path in the inspector.
        /// </summary>
        public string PathDisplayName { get; set; }

        #endregion
    }
}