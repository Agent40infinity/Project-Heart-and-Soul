// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextMemberFilter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Utils
{
    using System;

    /// <summary>
    ///   Flags to filter which members of a context should be used.
    /// </summary>
    [Flags]
    public enum ContextMemberFilter
    {
        /// <summary>
        ///   No members should be used.
        /// </summary>
        None,

        /// <summary>
        ///   Methods should be included.
        /// </summary>
        Methods = 1 << 0,

        /// <summary>
        ///   Properties should be included.
        /// </summary>
        Properties = 1 << 1,

        /// <summary>
        ///   Fields should be included.
        /// </summary>
        Fields = 1 << 2,

        /// <summary>
        ///   Sub contexts should be included.
        /// </summary>
        Contexts = 1 << 3,

        /// <summary>
        ///   Sub contexts should be handled, too, to include their members as well.
        /// </summary>
        Recursive = 1 << 4,

        /// <summary>
        ///   Triggers should be included.
        /// </summary>
        Triggers = 1 << 5,

        /// <summary>
        ///   All members of this context and all sub contexts recursively should be used.
        /// </summary>
        All = ~0
    }
}