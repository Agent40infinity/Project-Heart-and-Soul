namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;
    using System.Collections.Generic;
    using Slash.Unity.DataBind.Core.Data;

    /// <summary>
    ///     Base class of a collection data binding.
    /// </summary>
    public abstract class CollectionDataBindingBase
    {
        /// <summary>
        ///     Action to execute when the items were cleared.
        /// </summary>
        public Action ClearedItems;

        /// <summary>
        ///     Action to execute when the collection changed.
        /// </summary>
        public Action<Collection> CollectionChanged;

        /// <summary>
        ///     Collection this binding observes.
        /// </summary>
        protected DataBinding collectionBinding;

        private Collection collection;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="collectionBinding">Collection binding to observe.</param>
        protected CollectionDataBindingBase(DataBinding collectionBinding)
        {
            this.collectionBinding = collectionBinding;
        }

        /// <summary>
        ///     Current collection that the binding provides.
        /// </summary>
        public Collection Collection
        {
            get { return this.collection; }
            private set
            {
                if (value == this.collection)
                {
                    return;
                }

                if (this.collection != null)
                {
                    this.collection.ItemAdded -= this.OnItemAdded;
                    this.collection.ItemRemoved -= this.OnItemRemoved;
                    this.collection.ItemInserted -= this.OnItemInserted;
                    this.collection.ItemReplaced -= this.OnItemReplaced;
                    this.collection.ClearedItems -= this.OnClearedItems;
                }

                this.collection = value;

                if (this.collection != null)
                {
                    this.collection.ItemAdded += this.OnItemAdded;
                    this.collection.ItemRemoved += this.OnItemRemoved;
                    this.collection.ItemInserted += this.OnItemInserted;
                    this.collection.ItemReplaced += this.OnItemReplaced;
                    this.collection.ClearedItems += this.OnClearedItems;
                }

                if (this.CollectionChanged != null)
                {
                    this.CollectionChanged(this.collection);
                }
            }
        }

        /// <summary>
        ///     Disables the observer.
        /// </summary>
        public void Disable()
        {
            this.collectionBinding.ValueChanged -= this.OnCollectionChanged;
            this.Collection = null;
        }

        /// <summary>
        ///     Enables the observer.
        /// </summary>
        public void Enable()
        {
            this.collectionBinding.ValueChanged += this.OnCollectionChanged;
            this.Collection = this.collectionBinding.GetValue<Collection>();
        }

        /// <summary>
        ///     Called when an item was added to the collection.
        /// </summary>
        /// <param name="item">Item which was added.</param>
        protected abstract void OnItemAdded(object item);

        /// <summary>
        ///     Called when an item was inserted in the collection.
        /// </summary>
        /// <param name="item">Item which was inserted.</param>
        /// <param name="index">Index where item was inserted.</param>
        protected abstract void OnItemInserted(object item, int index);

        /// <summary>
        ///     Called when item was removed from the collection.
        /// </summary>
        /// <param name="item">Item which was removed from collection.</param>
        protected abstract void OnItemRemoved(object item);

        /// <summary>
        ///     Called when an item was replaced in the collection.
        /// </summary>
        /// <param name="index">Index at which the item was replaced.</param>
        /// <param name="previousItem">Replaced item.</param>
        /// <param name="newItem">New item.</param>
        protected abstract void OnItemReplaced(int index, object previousItem, object newItem);

        private void OnClearedItems(IEnumerable<object> items)
        {
            if (this.ClearedItems != null)
            {
                this.ClearedItems();
            }
        }

        private void OnCollectionChanged()
        {
            this.Collection = this.collectionBinding.GetValue<Collection>();
        }
    }

    /// <summary>
    ///     Non-generic collection data binding.
    /// </summary>
    public class CollectionDataBinding : CollectionDataBindingBase
    {
        /// <summary>
        ///     Action to execute when an item was added to the collection.
        /// </summary>
        public Action<object> ItemAdded;

        /// <summary>
        ///     Action to execute when an item was inserted to the collection.
        /// </summary>
        public Action<int, object> ItemInserted;

        /// <summary>
        ///     Action to execute when an item was removed from the collection.
        /// </summary>
        public Action<object> ItemRemoved;

        /// <summary>
        ///     Action to execute when an item was replaced in the collection.
        /// </summary>
        public Action<int, object, object> ItemReplaced;

        /// <inheritdoc />
        public CollectionDataBinding(DataBinding collectionBinding) : base(collectionBinding)
        {
        }

        /// <inheritdoc />
        protected override void OnItemAdded(object item)
        {
            if (this.ItemAdded != null)
            {
                this.ItemAdded(item);
            }
        }

        /// <inheritdoc />
        protected override void OnItemInserted(object item, int index)
        {
            if (this.ItemInserted != null)
            {
                this.ItemInserted(index, item);
            }
        }

        /// <inheritdoc />
        protected override void OnItemRemoved(object item)
        {
            if (this.ItemRemoved != null)
            {
                this.ItemRemoved(item);
            }
        }

        /// <inheritdoc />
        protected override void OnItemReplaced(int index, object previousItem, object newItem)
        {
            if (this.ItemReplaced != null)
            {
                this.ItemReplaced(index, previousItem, newItem);
            }
        }
    }

    /// <summary>
    ///     Utility class for a data binding of a collection.
    ///     Often we have to react on item added/removed/inserted events and those actions have to be
    ///     moved over to a new bound collection when it changes.
    ///     This class takes over the task to observe the collection and registers for its events.
    /// </summary>
    /// <typeparam name="TItem">Type of items in the collection.</typeparam>
    public class CollectionDataBinding<TItem> : CollectionDataBindingBase
    {
        /// <summary>
        ///     Action to execute when an item was added to the collection.
        /// </summary>
        public Action<TItem> ItemAdded;

        /// <summary>
        ///     Action to execute when an item was inserted to the collection.
        /// </summary>
        public Action<int, TItem> ItemInserted;

        /// <summary>
        ///     Action to execute when an item was removed from the collection.
        /// </summary>
        public Action<TItem> ItemRemoved;

        /// <summary>
        ///     Action to execute when an item was replaced in the collection.
        /// </summary>
        public Action<int, TItem, TItem> ItemReplaced;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="collectionBinding">Collection binding to observe.</param>
        public CollectionDataBinding(DataBinding collectionBinding)
            : base(collectionBinding)
        {
        }

        /// <inheritdoc />
        protected override void OnItemAdded(object item)
        {
            if (this.ItemAdded != null)
            {
                this.ItemAdded((TItem) item);
            }
        }

        /// <inheritdoc />
        protected override void OnItemInserted(object item, int index)
        {
            if (this.ItemInserted != null)
            {
                this.ItemInserted(index, (TItem) item);
            }
        }

        /// <inheritdoc />
        protected override void OnItemRemoved(object item)
        {
            if (this.ItemRemoved != null)
            {
                this.ItemRemoved((TItem) item);
            }
        }

        /// <inheritdoc />
        protected override void OnItemReplaced(int index, object previousItem, object newItem)
        {
            if (this.ItemReplaced != null)
            {
                this.ItemReplaced(index, (TItem) previousItem, (TItem) newItem);
            }
        }
    }
}