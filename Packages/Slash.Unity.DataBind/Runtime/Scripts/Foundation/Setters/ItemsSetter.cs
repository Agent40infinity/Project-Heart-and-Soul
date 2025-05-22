// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using System;
    using System.Collections;
    using Slash.Unity.DataBind.Core.Data;
    using UnityEngine;

    /// <summary>
    ///     Base class for a setter which uses a collection or an integer to determine how many
    ///     items are shown beneath the game object of the target behaviour.
    /// </summary>
    public abstract class ItemsSetter : ComponentSingleSetter<Transform, object>
    {
        #region Fields

        /// <summary>
        ///     Collection to visualize.
        /// </summary>
        private Collection collection;

        #endregion

        #region Properties

        /// <summary>
        ///     Collection to visualize.
        /// </summary>
        private Collection Collection
        {
            set
            {
                if (value == this.collection)
                {
                    return;
                }

                if (this.collection != null)
                {
                    // Remove from modifications of the collection.
                    this.collection.ItemAdded -= this.OnCollectionItemAdded;
                    this.collection.ItemInserted -= this.OnCollectionItemInserted;
                    this.collection.ItemRemoved -= this.OnCollectionItemRemoved;
                    this.collection.ItemReplaced -= this.OnCollectionItemReplacedd;
                    this.collection.Cleared -= this.OnCollectionCleared;
                }

                this.collection = value;

                if (this.collection != null)
                {
                    // Register for modifications of the collection.
                    this.collection.ItemAdded += this.OnCollectionItemAdded;
                    this.collection.ItemInserted += this.OnCollectionItemInserted;
                    this.collection.ItemRemoved += this.OnCollectionItemRemoved;
                    this.collection.ItemReplaced += this.OnCollectionItemReplacedd;
                    this.collection.Cleared += this.OnCollectionCleared;
                }
            }
        }

		protected Collection TargetCollection => collection;
        #endregion

        #region Methods

        /// <summary>
        ///     Clears all created items.
        /// </summary>
        protected abstract void ClearItems();

        /// <summary>
        ///     Creates an item for the specified item context.
        /// </summary>
        /// <param name="itemContext">Item context for the item to create.</param>
        /// <param name="itemIndex">Index of item to create.</param>
        protected abstract void CreateItem(object itemContext, int itemIndex);

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();

            // Clear callbacks.
            this.Collection = null;
        }

        /// <summary>
        ///     Called when the items of the control changed.
        /// </summary>
        protected virtual void OnItemsChanged()
        {
        }

        /// <inheritdoc />
        protected override void UpdateTargetValue(Transform target, object value)
        {
            // Clear items.
            this.ClearItems();

            // Create new items.
            this.CreateItems(value);
        }

        /// <summary>
        ///     Removes the item with the specified item context.
        /// </summary>
        /// <param name="itemContext">Item context of the item to remove.</param>
        protected abstract void RemoveItem(object itemContext);

        private void CreateItems(int numItems)
        {
            if (numItems <= 0)
            {
                return;
            }

            for (var index = 0; index < numItems; index++)
            {
                // Create game object for item.
                this.CreateItem(null, index);
            }

            this.OnItemsChanged();
        }

        private void CreateItems(IEnumerable itemContexts)
        {
            if (itemContexts == null)
            {
                return;
            }

            // Fill with objects from collection.
            var itemIndex = 0;
            foreach (var itemContext in itemContexts)
            {
                // Create game object for item.
                this.CreateItem(itemContext, itemIndex++);
            }

            this.OnItemsChanged();
        }

        private void CreateItems(object newValue)
        {
            if (newValue == null)
            {
                this.Collection = null;
                return;
            }

            // Check if collection or number.
            this.Collection = newValue as Collection;
            var enumerable = newValue as IEnumerable;
            if (enumerable != null)
            {
                this.CreateItems(enumerable);
            }
            else
            {
                try
                {
                    var intValue = Convert.ChangeType(newValue, typeof(int));
                    if (intValue != null)
                    {
                        var numItems = (int) intValue;
                        this.CreateItems(numItems);
                    }
                }
                catch (InvalidCastException)
                {
                    Debug.LogWarning("Data value is neither a collection nor a number.", this);
                }
            }
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            // Clear collection.
            this.Collection = null;
        }

        private void OnCollectionCleared()
        {
            this.ClearItems();
        }

        private void OnCollectionItemAdded(object item)
        {
            // Create game object for item.
            this.CreateItem(item, this.collection.Count);
            this.OnItemsChanged();
        }

        private void OnCollectionItemInserted(object item, int index)
        {
            // Create game object for item.
            this.CreateItem(item, index);
            this.OnItemsChanged();
        }

        private void OnCollectionItemRemoved(object itemContext)
        {
            // Remove item for this context.
            this.RemoveItem(itemContext);

            this.OnItemsChanged();
        }

        private void OnCollectionItemReplacedd(int index, object previousItemContext, object newItemContext)
        {
            // Remove item for this context.
            this.RemoveItem(previousItemContext);

            // Create game object for item.
            this.CreateItem(newItemContext, index);

            this.OnItemsChanged();
        }

        #endregion
    }
}