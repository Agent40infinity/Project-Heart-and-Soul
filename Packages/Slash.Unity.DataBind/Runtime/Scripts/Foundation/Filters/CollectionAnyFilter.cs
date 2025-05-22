namespace Slash.Unity.DataBind.Foundation.Filters
{
    using System;
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
        ///    Data provider for an Any aggregation of the specified collection.
        /// </summary>
        /// <typeparam name="TItem">Type of item in the collection.</typeparam>
        /// <typeparam name="TData">Data type to perform predicate for Any operation on.</typeparam>
        /// <param name="collection">Collection to perform operation on.</param>
        /// <param name="itemDataProviderFunc">Function to get the data provider of an item that provides the value to use.</param>
        /// <param name="predicate">Function to perform on each item to check if it fits.</param>
        /// <returns>Data provider for an Any aggregation of the specified collection.</returns>
        public static CollectionAggregation<TItem, TData, bool> Any<TItem, TData>(Collection<TItem> collection,
            Func<TItem, IDataProvider<TData>> itemDataProviderFunc, Func<TData, bool> predicate) where TItem : IDataContext
        {
            return new CollectionAggregation<TItem, TData, bool>(collection, itemDataProviderFunc, items => items.Any(predicate));
        }

        /// <summary>
        ///    Data provider for an Any aggregation of the specified collection.
        /// </summary>
        /// <typeparam name="TItem">Type of item in the collection.</typeparam>
        /// <typeparam name="TData">Data type to perform predicate for Any operation on.</typeparam>
        /// <param name="collection">Collection to perform operation on.</param>
        /// <param name="itemDataFunc">Function to get the data of an item to use.</param>
        /// <param name="predicate">Function to perform on each item to check if it fits.</param>
        /// <returns>Data provider for an Any aggregation of the specified collection.</returns>
        public static CollectionAggregation<TItem, TData, bool> Any<TItem, TData>(Collection<TItem> collection,
            Func<TItem, TData> itemDataFunc, Func<TData, bool> predicate)
        {
            return new CollectionAggregation<TItem, TData, bool>(collection, itemDataFunc,
                items => items.Any(predicate));
        }
    }

    /// <summary>
    ///   Data provider which checks if any item in a collection fulfills a predicate.
    /// </summary>
    /// <typeparam name="TItem">Type of items in collection.</typeparam>
    public class CollectionAnyFilter<TItem> : CollectionAnyFilter<TItem, bool> where TItem : IDataContext
    {
        /// <inheritdoc />
        public CollectionAnyFilter(Collection<TItem> collection, Func<TItem, IDataProvider<bool>> itemDataProviderFunc) : base(collection, itemDataProviderFunc, data => data)
        {
        }
    }

    /// <summary>
    ///   Data provider which checks if any item in a collection fulfills a predicate.
    /// </summary>
    /// <typeparam name="TItem">Type of items in collection.</typeparam>
    /// <typeparam name="TData">Type of data the items provide.</typeparam>
    public class CollectionAnyFilter<TItem, TData> : Property<bool> where TItem : IDataContext
    {
        private readonly Func<TItem, IDataProvider<TData>> itemDataProviderFunc;

        private readonly CollectionObserver<TItem> observer;

        private readonly Func<TData, bool> predicate;

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="collection">Collection to create aggregated value from.</param>
        /// <param name="itemDataProviderFunc">Function to get the data provider of an item which will provide the item value.</param>
        /// <param name="predicate">Predicate to check items against.</param>
        public CollectionAnyFilter(Collection<TItem> collection, Func<TItem, IDataProvider<TData>> itemDataProviderFunc,
            Func<TData, bool> predicate)
        {
            this.observer = new CollectionObserver<TItem>();
            this.observer.Init(collection);
            this.observer.RegisterItem += this.OnRegisterItem;
            this.observer.UnregisterItem += this.OnUnregisterItem;
            this.itemDataProviderFunc = itemDataProviderFunc;
            this.predicate = predicate;
        }

        private void OnItemValueChanged()
        {
            this.UpdateValue();
        }

        private void OnRegisterItem(TItem item)
        {
            var itemDataProvider = this.itemDataProviderFunc(item);
            itemDataProvider.ValueChanged += this.OnItemValueChanged;
            this.UpdateValue();
        }

        private void OnUnregisterItem(TItem item)
        {
            var itemDataProvider = this.itemDataProviderFunc(item);
            itemDataProvider.ValueChanged -= this.OnItemValueChanged;
            this.UpdateValue();
        }

        private void UpdateValue()
        {
            this.Value = this.observer.Collection.Any(item =>
            {
                var data = this.itemDataProviderFunc(item);
                return this.predicate(data.Value);
            });
        }
    }
}