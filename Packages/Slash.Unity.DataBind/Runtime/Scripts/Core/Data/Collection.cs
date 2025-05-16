// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Collection.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Collection with events to monitor if an item was added/removed.
    /// </summary>
    public abstract class Collection : IEnumerable, IDataProvider
    {
        /// <summary>
        ///     Delegate for Cleared event.
        /// </summary>
        public delegate void ClearedDelegate();

        /// <summary>
        ///     Delegate for ClearedItems event.
        /// </summary>
        /// <param name="items">Removed items.</param>
        public delegate void ClearedItemsDelegate(IEnumerable<object> items);

        /// <summary>
        ///     Delegate for ItemAdded event.
        /// </summary>
        /// <param name="item">Item which was added.</param>
        public delegate void ItemAddedDelegate(object item);

        /// <summary>
        ///     Delegate for ItemInserted event.
        /// </summary>
        /// <param name="item">Item which was inserted.</param>
        /// <param name="index">Index the item was inserted.</param>
        public delegate void ItemInsertedDelegate(object item, int index);

        /// <summary>
        ///     Delegate for ItemRemoved event.
        /// </summary>
        /// <param name="item">Item which was removed.</param>
        public delegate void ItemRemovedDelegate(object item);

        /// <summary>
        ///     Delegate for ItemReplaced event.
        /// </summary>
        /// <param name="index">Index at which the item was replaced.</param>
        /// <param name="previousItem">Item which was replaced.</param>
        /// <param name="newItem">New item at the specified index.</param>
        public delegate void ItemReplacedDelegate(int index, object previousItem, object newItem);

        /// <summary>
        ///     Number of items in the collection.
        /// </summary>
        private readonly Property<int> countProperty = new Property<int>();

        /// <summary>
        ///     Number of items in the collection.
        /// </summary>
        public int Count
        {
            get { return this.countProperty.Value; }
            protected set { this.countProperty.Value = value; }
        }

        /// <summary>
        ///     Returns the item of the collection at the specified index.
        /// </summary>
        /// <param name="index">Index of item to return.</param>
        /// <returns>Item at specified index.</returns>
        public object this[int index]
        {
            get { return this.GetItem(index); }
            set { this.SetItem(index, value); }
        }

        /// <summary>
        ///     Type of items in this collection.
        /// </summary>
        public abstract Type ItemType { get; }

        /// <summary>
        ///     Called when the collection changed.
        /// </summary>
        public event ValueChangedDelegate ValueChanged;

        /// <inheritdoc />
        public bool IsInitialized
        {
            get { return true; }
        }

        /// <summary>
        ///     Current data value.
        /// </summary>
        public abstract object Value { get; }

        /// <summary>
        ///     Returns the enumerator of the collection.
        /// </summary>
        /// <returns>Enumerator of the collection.</returns>
        public IEnumerator GetEnumerator()
        {
            return this.GetObjectEnumerator();
        }

        /// <summary>
        ///     Called when the collection was cleared.
        /// </summary>
        public event ClearedDelegate Cleared;

        /// <summary>
        ///     Called when the collection was cleared.
        /// </summary>
        public event ClearedItemsDelegate ClearedItems;

        /// <summary>
        ///     Called when an item was added.
        /// </summary>
        public event ItemAddedDelegate ItemAdded;

        /// <summary>
        ///     Called when an item was inserted.
        /// </summary>
        public event ItemInsertedDelegate ItemInserted;

        /// <summary>
        ///     Called when an item was removed.
        /// </summary>
        public event ItemRemovedDelegate ItemRemoved;

        /// <summary>
        ///     Called when an item was replaced.
        /// </summary>
        public event ItemReplacedDelegate ItemReplaced;

        /// <summary>
        ///     Adds a new item to the collection.
        /// </summary>
        /// <returns>New item.</returns>
        public abstract object AddNewItem();

        /// <summary>
        ///     Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if the item was removed; false, if it didn't exist in the collection.</returns>
        public abstract bool Remove(object item);

        /// <summary>
        ///   Returns the current item count in this collection.
        /// </summary>
        /// <returns>Current item count in this collection.</returns>
        protected abstract int GetCount();

        /// <summary>
        ///     Returns the item at the specified index.
        /// </summary>
        /// <param name="index">Index to get item for.</param>
        /// <returns>Item at the specified index.</returns>
        protected abstract object GetItem(int index);

        /// <summary>
        ///     Returns an object enumerator for this collection.
        /// </summary>
        /// <returns>Object enumerator over this collection.</returns>
        protected abstract IEnumerator GetObjectEnumerator();

        /// <summary>
        ///     Called when the collection was cleared.
        /// </summary>
        /// <param name="items">Removed items.</param>
        protected virtual void OnClearedItems(IEnumerable<object> items)
        {
            var handler = this.ClearedItems;
            this.UpdateCount();
            if (handler != null)
            {
                handler(items);
            }

            this.OnCleared();
        }

        /// <summary>
        ///     Called when an item was added.
        /// </summary>
        /// <param name="item">Item which was added.</param>
        protected void OnItemAdded(object item)
        {
            var handler = this.ItemAdded;
            this.UpdateCount();
            if (handler != null)
            {
                handler(item);
            }

            this.OnValueChanged();
        }

        /// <summary>
        ///     Called when an item was inserted at a specific position.
        /// </summary>
        /// <param name="item">Item which was inserted.</param>
        /// <param name="index">Index the item was inserted.</param>
        protected virtual void OnItemInserted(object item, int index)
        {
            var handler = this.ItemInserted;
            this.UpdateCount();
            if (handler != null)
            {
                handler(item, index);
            }

            this.OnValueChanged();
        }

        /// <summary>
        ///     Called when an item was removed.
        /// </summary>
        /// <param name="item">Item which was removed.</param>
        protected void OnItemRemoved(object item)
        {
            var handler = this.ItemRemoved;
            this.UpdateCount();
            if (handler != null)
            {
                handler(item);
            }

            this.OnValueChanged();
        }

        /// <summary>
        ///     Called when an item was replaced.
        /// </summary>
        /// <param name="index">Index where item was replaced.</param>
        /// <param name="previousItem">Item which was replaced.</param>
        /// <param name="newItem">New item that replaced previous item.</param>
        protected void OnItemReplaced(int index, object previousItem, object newItem)
        {
            var handler = this.ItemReplaced;
            if (handler != null)
            {
                handler(index, previousItem, newItem);
            }
        }

        /// <summary>
        ///     Called when the collection changed.
        /// </summary>
        protected virtual void OnValueChanged()
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        ///     Sets the item at the specified index of the collection.
        /// </summary>
        /// <param name="index">Index to set item for.</param>
        /// <param name="value">Value to set at specified index.</param>
        protected abstract void SetItem(int index, object value);

        /// <summary>
        ///     Called when the collection was cleared.
        /// </summary>
        private void OnCleared()
        {
            var handler = this.Cleared;
            if (handler != null)
            {
                handler();
            }

            this.OnValueChanged();
        }

        private void UpdateCount()
        {
            this.Count = this.GetCount();
        }
    }

    /// <summary>
    ///     Generic collection with events to monitor when an item was added/removed.
    /// </summary>
    /// <typeparam name="T">Type of items in the collection.</typeparam>
    public sealed class Collection<T> : Collection, IList<T>
    {
        private readonly List<T> items = new List<T>();

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="items">Items to initialize the collection with.</param>
        public Collection(IEnumerable<T> items)
        {
            this.items = new List<T>(items);
            this.Count = this.items.Count;
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        public Collection()
        {
        }

        /// <summary>
        ///     Type of items in this collection.
        /// </summary>
        public override Type ItemType
        {
            get { return typeof(T); }
        }

        /// <summary>
        ///     Current data value.
        /// </summary>
        public override object Value
        {
            get { return this.items; }
        }

        /// <summary>
        ///     Indicates if the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public new T this[int index]
        {
            get { return this.items[index]; }
            set { this.SetItem(index, value); }
        }

        /// <summary>
        ///     Adds the specified item to the collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(T item)
        {
            this.items.Add(item);
            this.OnItemAdded(item);
        }

        /// <summary>
        ///     Clears the collection.
        /// </summary>
        public void Clear()
        {
            if (this.items.Count == 0)
            {
                return;
            }

            var removedItems = new List<object>(this.items.Cast<object>());
            this.items.Clear();
            this.OnClearedItems(removedItems);
        }

        /// <summary>
        ///     Indicates if the collection contains the specified item.
        /// </summary>
        /// <param name="item">Item to check.</param>
        /// <returns>True if the specified item exists in the collection; otherwise, false.</returns>
        public bool Contains(T item)
        {
            return this.items.Contains(item);
        }

        /// <summary>
        ///     Copies the collection to the specified index in the specified array.
        /// </summary>
        /// <param name="array">Array to copy this collection to.</param>
        /// <param name="arrayIndex">Array index to start to put copies in the array.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            foreach (var item in this.items)
            {
                array[arrayIndex++] = item;
            }
        }

        /// <summary>
        ///     Returns the index of the specified item in the collection.
        /// </summary>
        /// <param name="item">Item to get index of.</param>
        /// <returns>Index of specified item.</returns>
        public int IndexOf(T item)
        {
            return this.items.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            this.items.Insert(index, item);
            this.OnItemInserted(item, index);
        }

        /// <summary>
        ///     Removes the specified item from the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>True if the item was removed; false, if it didn't exist in the collection.</returns>
        public bool Remove(T item)
        {
            if (!this.items.Remove(item))
            {
                return false;
            }

            this.OnItemRemoved(item);

            return true;
        }

        /// <summary>
        ///     Removes the item at the specified index.
        /// </summary>
        /// <param name="index">Index to remove item at.</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.items.Count)
            {
                return;
            }

            var item = this.items[index];
            this.items.RemoveAt(index);

            this.OnItemRemoved(item);
        }

        /// <inheritdoc />
        public new IEnumerator<T> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <summary>
        ///     Adds a new item to the collection.
        /// </summary>
        /// <returns>New item.</returns>
        public override object AddNewItem()
        {
            var newItem = Activator.CreateInstance<T>();
            this.Add(newItem);
            return newItem;
        }

        /// <inheritdoc />
        public override bool Remove(object item)
        {
            if (!(item is T))
            {
                return false;
            }

            return this.Remove((T) item);
        }

        /// <inheritdoc />
        protected override int GetCount()
        {
            return this.items.Count;
        }

        /// <inheritdoc />
        protected override object GetItem(int index)
        {
            return this[index];
        }

        /// <inheritdoc />
        protected override IEnumerator GetObjectEnumerator()
        {
            return this.items.GetEnumerator();
        }

        /// <inheritdoc />
        protected override void SetItem(int index, object value)
        {
            var oldValue = this.items[index];
            this.items[index] = (T) value;
            this.OnItemReplaced(index, oldValue, value);
        }
    }
}