namespace Slash.Unity.DataBind.Foundation.Utils
{
    using System;
    using System.Collections.Generic;
    using Slash.Unity.DataBind.Core.Data;

    /// <summary>
    ///     Utility class to observe the items of a collection.
    ///     Can be used e.g. when the single items have an event that should be forwarded to a common context event.
    /// </summary>
    /// <typeparam name="TItem">Type of items in the collection.</typeparam>
    public class CollectionObserver<TItem>
    {
        private readonly List<TItem> registeredItems = new List<TItem>();

        /// <summary>
        ///     Collection which is observed.
        /// </summary>
        public Collection<TItem> Collection { get; private set; }

        /// <summary>
        ///     Action to call after an item enters the collection.
        /// </summary>
        public event Action<TItem> RegisterItem;

        /// <summary>
        ///     Action to call before an item leaves the collection.
        /// </summary>
        public event Action<TItem> UnregisterItem;

        /// <summary>
        ///     Deinitializes the observer.
        /// </summary>
        public void Deinit()
        {
            this.OnCleared();

            if (this.Collection != null)
            {
                this.Collection.ItemAdded -= this.OnItemAdded;
                this.Collection.ItemInserted -= this.OnItemInserted;
                this.Collection.ItemRemoved -= this.OnItemRemoved;
                this.Collection.Cleared -= this.OnCleared;
                this.Collection = null;
            }
        }

        /// <summary>
        ///     Initializes the observer.
        /// </summary>
        /// <param name="collectionToObserve">Collection to observe.</param>
        public void Init(Collection<TItem> collectionToObserve)
        {
            this.Collection = collectionToObserve;
            this.Collection.ItemAdded += this.OnItemAdded;
            this.Collection.ItemInserted += this.OnItemInserted;
            this.Collection.ItemRemoved += this.OnItemRemoved;
            this.Collection.Cleared += this.OnCleared;

            this.registeredItems.Clear();
            foreach (var item in collectionToObserve)
            {
                this.OnRegisterItem(item);
                this.registeredItems.Add(item);
            }
        }

        private void OnCleared()
        {
            foreach (var item in this.registeredItems)
            {
                this.OnUnregisterItem(item);
            }

            this.registeredItems.Clear();
        }

        private void OnItemAdded(object item)
        {
            var castedItem = (TItem) item;
            this.OnRegisterItem(castedItem);
            this.registeredItems.Add(castedItem);
        }

        private void OnItemInserted(object item, int index)
        {
            var castedItem = (TItem) item;
            this.OnRegisterItem(castedItem);
            this.registeredItems.Insert(index, castedItem);
        }

        private void OnItemRemoved(object item)
        {
            var castedItem = (TItem) item;
            this.OnUnregisterItem(castedItem);
            this.registeredItems.Remove(castedItem);
        }

        private void OnRegisterItem(TItem item)
        {
            var handler = this.RegisterItem;
            if (handler != null)
            {
                handler(item);
            }
        }

        private void OnUnregisterItem(TItem item)
        {
            var handler = this.UnregisterItem;
            if (handler != null)
            {
                handler(item);
            }
        }
    }
}