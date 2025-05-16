namespace Slash.Unity.DataBind.Core.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///   Utility methods for collections.
    /// </summary>
    public static class CollectionUtils
    {
        /// <summary>
        ///   Creates a separated string from the items of a collection.
        ///   Mostly used for debugging issues.
        /// </summary>
        /// <param name="collection">Collection of items to create a string from.</param>
        /// <param name="separator">String to use to separate the item strings.</param>
        /// <typeparam name="T">Type of items in collection.</typeparam>
        /// <returns></returns>
        public static string Implode<T>(this IEnumerable<T> collection, string separator)
        {
            return collection != null
                ? string.Join(separator, collection.Select(item => item != null ? item.ToString() : "<null>").ToArray())
                : string.Empty;
        }

        /// <summary>
        ///   Creates a separated string from the items of a collection.
        ///   Mostly used for debugging issues.
        /// </summary>
        /// <param name="collection">Collection of items to create a string from.</param>
        /// <param name="separator">String to use to separate the item strings.</param>
        /// <returns>String created from the items which are separated by the provided separator.</returns>
        public static string Implode(this IEnumerable collection, string separator)
        {
            return collection != null
                ? collection.Cast<object>().Implode(separator)
                : string.Empty;
        }
    }
}