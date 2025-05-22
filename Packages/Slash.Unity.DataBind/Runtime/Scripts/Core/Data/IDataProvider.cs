// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataProvider.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
    /// <summary>
    ///     Delegate for ValueChanged event.
    /// </summary>
    public delegate void ValueChangedDelegate();

    /// <summary>
    ///     Interface for any kind of provider for a data value.
    /// </summary>
    public interface IDataProvider
    {
        #region Events

        /// <summary>
        ///     Called when the value of the property changed.
        /// </summary>
        event ValueChangedDelegate ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        ///     Current data value.
        /// </summary>
        object Value { get; }

        /// <summary>
        ///     Indicates if data provider is initialized to provide a valid value.
        /// </summary>
        bool IsInitialized { get; }

        #endregion
    }

    /// <summary>
    ///     Interface for any kind of provider of a data value of a specific type.
    /// </summary>
    /// <typeparam name="T">Type of provided data.</typeparam>
    public interface IDataProvider<T> : IDataProvider
    {
        /// <summary>
        ///     Current value of provided data.
        /// </summary>
        new T Value { get; }
    }
}