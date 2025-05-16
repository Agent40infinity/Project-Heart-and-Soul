namespace Slash.Unity.DataBind.Foundation.Triggers
{
    using System;
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    ///     Provides Unity events for the data events of a bound collection.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Triggers/[DB] Collection Unity Events")]
    public class CollectionUnityEvents : DataBindingOperator
    {
        #region Bound Collection

        /// <summary>
        ///     Collection to observe.
        /// </summary>
        [Tooltip("Collection to observe")]
        public DataBinding Collection;

        #endregion

        #region Unity Events

        /// <summary>
        ///     Called when the bound collection was cleared.
        /// </summary>
        [Tooltip("Called when the bound collection was cleared")]
        public ClearedItemsEvent ClearedItems;

        /// <summary>
        ///     Called when the bound collection changed.
        /// </summary>
        [Tooltip("Called when the bound collection changed")]
        public CollectionChangedEvent CollectionChanged;

        /// <summary>
        ///     Called when an item was added to the bound collection.
        /// </summary>
        [Tooltip("Called when an item was added to the bound collection")]
        public ItemAddedEvent ItemAdded;

        /// <summary>
        ///     Called when an item was inserted in the bound collection.
        /// </summary>
        [Tooltip("Called when an item was inserted in the bound collection")]
        public ItemInsertedEvent ItemInserted;

        /// <summary>
        ///     Called when an item was removed from the bound collection.
        /// </summary>
        [Tooltip("Called when an item was removed from the bound collection")]
        public ItemRemovedEvent ItemRemoved;

        #endregion

        private CollectionDataBinding collectionDataBinding;

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();

            this.RemoveBinding(this.Collection);

            this.collectionDataBinding = null;
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            this.collectionDataBinding.Disable();
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            this.collectionDataBinding.Enable();
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();

            this.collectionDataBinding = new CollectionDataBinding(this.Collection)
            {
                ItemAdded = this.OnItemAdded,
                ItemInserted = this.OnItemInserted,
                ItemRemoved = this.OnItemRemoved,
                ClearedItems = this.OnClearedItems,
                CollectionChanged = this.OnCollectionChanged
            };

            this.AddBinding(this.Collection);
        }

        private void OnClearedItems()
        {
            this.ClearedItems.Invoke();
        }

        private void OnCollectionChanged(Collection collection)
        {
            this.CollectionChanged.Invoke(collection);
        }

        private void OnItemAdded(object item)
        {
            this.ItemAdded.Invoke(item);
        }

        private void OnItemInserted(int index, object item)
        {
            this.ItemInserted.Invoke(index, item);
        }

        private void OnItemRemoved(object item)
        {
            this.ItemRemoved.Invoke(item);
        }

        /// <summary>
        ///   Unity event for ItemAdded event of collection.
        /// </summary>
        [Serializable]
        public class ItemAddedEvent : UnityEvent<object>
        {
        }

        /// <summary>
        ///   Unity event for ItemInserted event of collection.
        /// </summary>
        [Serializable]
        public class ItemInsertedEvent : UnityEvent<int, object>
        {
        }

        /// <summary>
        ///   Unity event for ItemRemoved event of collection.
        /// </summary>
        [Serializable]
        public class ItemRemovedEvent : UnityEvent<object>
        {
        }

        /// <summary>
        ///   Unity event for ClearedItems event of collection.
        /// </summary>
        [Serializable]
        public class ClearedItemsEvent : UnityEvent
        {
        }

        /// <summary>
        ///   Unity event for CollectionChanged event of collection.
        /// </summary>
        [Serializable]
        public class CollectionChangedEvent : UnityEvent<Collection>
        {
        }
    }
}