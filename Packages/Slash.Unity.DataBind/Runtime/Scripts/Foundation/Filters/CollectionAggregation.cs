namespace Slash.Unity.DataBind.Foundation.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Foundation.Utils;

    /// <summary>
    ///   Class to create a data provider which is an aggregation of a data collection.
    ///   LINQ-like syntax to use with collections.
    /// </summary>
    public static partial class CollectionAggregation
    {
        /// <summary>
        ///    Data provider which value is the sum of the items inside the specified collection.
        /// </summary>
        /// <typeparam name="TItem">Type of item in the collection.</typeparam>
        /// <param name="collection">Collection to build sum from.</param>
        /// <param name="itemDataProviderFunc">Function to get the data provider of an item that provides the value to sum up.</param>
        /// <returns>Data provider which provides the sum for the specified collection.</returns>
        public static CollectionAggregation<TItem, int, int> Sum<TItem>(Collection<TItem> collection,
            Func<TItem, IDataProvider<int>> itemDataProviderFunc) where TItem : IDataContext
        {
            return new CollectionAggregation<TItem, int, int>(collection, itemDataProviderFunc, items => items.Sum());
        }

        /// <summary>
        ///    Data provider which value is the sum of the items inside the specified collection.
        /// </summary>
        /// <typeparam name="TItem">Type of item in the collection.</typeparam>
        /// <param name="collection">Collection to build sum from.</param>
        /// <param name="itemDataProviderFunc">Function to get the data provider of an item that provides the value to sum up.</param>
        /// <returns>Data provider which provides the sum for the specified collection.</returns>
        public static CollectionAggregation<TItem, float, float> Sum<TItem>(Collection<TItem> collection,
            Func<TItem, IDataProvider<float>> itemDataProviderFunc) where TItem : IDataContext
        {
            return new CollectionAggregation<TItem, float, float>(collection, itemDataProviderFunc, items => items.Sum());
        }

        /// <summary>
        ///    Data provider which value is the sum of the items inside the specified collection.
        /// </summary>
        /// <typeparam name="TItem">Type of item in the collection.</typeparam>
        /// <param name="collection">Collection to build sum from.</param>
        /// <param name="itemDataFunc">Function to get the data of an item to sum up.</param>
        /// <returns>Data provider which provides the sum for the specified collection.</returns>
        public static CollectionAggregation<TItem, int, int> Sum<TItem>(Collection<TItem> collection,
            Func<TItem, int> itemDataFunc) where TItem : IDataContext
        {
            return new CollectionAggregation<TItem, int, int>(collection, itemDataFunc, items => items.Sum());
        }

        /// <summary>
        ///    Data provider which value is the sum of the items inside the specified collection.
        /// </summary>
        /// <typeparam name="TItem">Type of item in the collection.</typeparam>
        /// <param name="collection">Collection to build sum from.</param>
        /// <param name="itemDataFunc">Function to get the data of an item to sum up.</param>
        /// <returns>Data provider which provides the sum for the specified collection.</returns>
        public static CollectionAggregation<TItem, float, float> Sum<TItem>(Collection<TItem> collection,
            Func<TItem, float> itemDataFunc) where TItem : IDataContext
        {
            return new CollectionAggregation<TItem, float, float>(collection, itemDataFunc, items => items.Sum());
        }
    }

    /// <summary>
    ///   Generic aggregation data provider for a collection.
    /// </summary>
    /// <typeparam name="TItem">Type of items in collection.</typeparam>
    /// <typeparam name="TData">Type of item data to use for aggregation.</typeparam>
    /// <typeparam name="TResult">Type of provided data.</typeparam>
    public class CollectionAggregation<TItem, TData, TResult> : Property<TResult>
    {
        private readonly Func<TItem, TData> itemDataFunc;

        private readonly Func<TItem, IDataProvider<TData>> itemDataProviderFunc;

        private readonly Func<IEnumerable<TData>, TResult> linqFunc;

        private readonly CollectionObserver<TItem> observer;

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="collection">Collection to create aggregated value from.</param>
        /// <param name="itemDataFunc">Function to get the data value of an item.</param>
        /// <param name="linqFunc">Aggregation function that uses the item values to build a resulting value.</param>
        public CollectionAggregation(Collection<TItem> collection, Func<TItem, TData> itemDataFunc,
            Func<IEnumerable<TData>, TResult> linqFunc)
            : this(collection, null, itemDataFunc, linqFunc)
        {
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="collection">Collection to create aggregated value from.</param>
        /// <param name="itemDataProviderFunc">Function to get the data provider of an item which will provide the item value.</param>
        /// <param name="linqFunc">Aggregation function that uses the item values to build a resulting value.</param>
        public CollectionAggregation(Collection<TItem> collection,
            Func<TItem, IDataProvider<TData>> itemDataProviderFunc, Func<IEnumerable<TData>, TResult> linqFunc)
            : this(collection, itemDataProviderFunc, item => itemDataProviderFunc(item).Value, linqFunc)
        {
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="collection">Collection to create aggregated value from.</param>
        /// <param name="itemDataProviderFunc">Function to get the data provider of an item which will provide the item value.</param>
        /// <param name="itemDataFunc">Function to get the data value of an item.</param>
        /// <param name="linqFunc">Aggregation function that uses the item values to build a resulting value.</param>
        public CollectionAggregation(Collection<TItem> collection,
            Func<TItem, IDataProvider<TData>> itemDataProviderFunc, Func<TItem, TData> itemDataFunc,
            Func<IEnumerable<TData>, TResult> linqFunc)
        {
            this.itemDataProviderFunc = itemDataProviderFunc;
            this.itemDataFunc = itemDataFunc;
            this.linqFunc = linqFunc;

            this.observer = new CollectionObserver<TItem>();
            this.observer.Init(collection);
            this.observer.RegisterItem += this.OnRegisterItem;
            this.observer.UnregisterItem += this.OnUnregisterItem;

            this.UpdateValue();
        }

        private void OnItemValueChanged()
        {
            this.UpdateValue();
        }

        private void OnRegisterItem(TItem item)
        {
            if (this.itemDataProviderFunc != null)
            {
                var itemDataProvider = this.itemDataProviderFunc(item);
                itemDataProvider.ValueChanged += this.OnItemValueChanged;
            }

            this.UpdateValue();
        }

        private void OnUnregisterItem(TItem item)
        {
            if (this.itemDataProviderFunc != null)
            {
                var itemDataProvider = this.itemDataProviderFunc(item);
                itemDataProvider.ValueChanged -= this.OnItemValueChanged;
            }

            this.UpdateValue();
        }

        private void UpdateValue()
        {
            this.Value = this.observer.Collection != null
                ? this.linqFunc(this.observer.Collection.Select(this.itemDataFunc))
                : default(TResult);
        }
    }
}