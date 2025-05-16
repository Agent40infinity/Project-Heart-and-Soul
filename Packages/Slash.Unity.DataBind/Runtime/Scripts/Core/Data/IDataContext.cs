namespace Slash.Unity.DataBind.Core.Data
{
    using System;

    /// <summary>
    ///   Interface of a data context.
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        ///     Returns the value at the specified path.
        /// </summary>
        /// <param name="path">Path to get value for.</param>
        /// <returns>Current value at specified path.</returns>
        object GetValue(string path);

        /// <summary>
        ///     Registers a callback at the specified path of the context.
        /// </summary>
        /// <param name="path">Path to register for.</param>
        /// <param name="onValueChanged">Callback to invoke when value at the specified path changed.</param>
        /// <exception cref="ArgumentException">Thrown if path is invalid for this context.</exception>
        /// <returns>Current value at specified path.</returns>
        object RegisterListener(string path, Action<object> onValueChanged);

        /// <summary>
        ///     Removes the callback from the specified path of the context.
        /// </summary>
        /// <param name="path">Path to remove callback from.</param>
        /// <param name="onValueChanged">Callback to remove.</param>
        /// <exception cref="ArgumentException">Thrown if path is invalid for this context.</exception>
        void RemoveListener(string path, Action<object> onValueChanged);

        /// <summary>
        ///     Sets the specified value at the specified path.
        /// </summary>
        /// <param name="path">Path to set the data value at.</param>
        /// <exception cref="ArgumentException">Thrown if path is invalid for this context.</exception>
        /// <exception cref="InvalidOperationException">Thrown if data at specified path can't be changed.</exception>
        /// <param name="value">Value to set.</param>
        void SetValue(string path, object value);
    }
}