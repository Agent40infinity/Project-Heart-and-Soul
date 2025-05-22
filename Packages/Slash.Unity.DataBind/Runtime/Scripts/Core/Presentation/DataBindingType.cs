// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBindingType.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    /// <summary>
    ///   Type of data binding.
    /// </summary>
    public enum DataBindingType
    {
        /// <summary>
        ///   Data is fetched from a data context.
        /// </summary>
        Context,

        /// <summary>
        ///   Data is taken from a specific provider.
        /// </summary>
        Provider,

        /// <summary>
        ///   Data is a constant value.
        /// </summary>
        Constant,

        /// <summary>
        ///   Data is an object reference.
        /// </summary>
        Reference,
		
		/// <summary>
		///   Data is a localized string.
		/// </summary>
		LocalizedString,
    }
}