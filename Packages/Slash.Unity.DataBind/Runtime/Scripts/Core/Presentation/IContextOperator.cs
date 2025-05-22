// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IContextOperator.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    /// <summary>
    ///   Interface for a class which works with a data context.
    /// </summary>
    public interface IContextOperator
    {
        #region Public Methods and Operators

        /// <summary>
        ///   Called when the context this operator works with changed.
        /// </summary>
        void OnContextChanged();

        #endregion
    }
}