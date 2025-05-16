namespace Slash.Unity.DataBind.UI.Unity.Setters
{
    using System;
    using System.Collections.Generic;

    using Slash.Unity.DataBind.Foundation.Providers.Formatters;
    using Slash.Unity.DataBind.Foundation.Setters;

    using UnityEngine;

    /// <summary>
    ///   Sets the items of a LayoutGroup depending on the items of the collection data value.
    ///   Creates the items one after another instead of all at once.
    ///   If you don't use any parent references in your item contexts,
    ///   use <see cref="SmoothCollectionChangesFormatter"/> instead.
    /// </summary>
    [Obsolete("Use SmoothCollectionChangesFormatter and a GameObjectItemsSetter instead")]
    public class SmoothLayoutGroupItemsSetter : GameObjectItemsSetter
    {
        #region Fields

        /// <summary>
        ///   Items to add one by one to the smoothed version of the collection.
        /// </summary>
        private readonly List<QueuedItem> queue = new List<QueuedItem>();

        /// <summary>
        ///   Interval between two items to be added, in seconds.
        /// </summary>
        [Tooltip("Interval between two items to be added, in seconds.")]
        public float Interval;

        /// <summary>
        ///   Time remaining before the next item is added to the smoothed collection, in seconds.
        /// </summary>
        private float timeRemaining;

        #endregion

        #region Methods

        /// <summary>
        ///   Clears all created items.
        /// </summary>
        protected sealed override void ClearItems()
        {
            base.ClearItems();

            this.queue.Clear();
            this.timeRemaining = 0;
        }

        /// <summary>
        ///   Creates an item for the specified item context.
        /// </summary>
        /// <param name="itemContext">Item context for the item to create.</param>
        /// <param name="itemIndex">Index of item to create.</param>
        protected sealed override void CreateItem(object itemContext, int itemIndex)
        {
            this.queue.Add(new QueuedItem { ItemContext = itemContext, ItemIndex = itemIndex });
        }

        /// <summary>
        ///   Removes the item with the specified item context.
        /// </summary>
        /// <param name="itemContext">Item context of the item to remove.</param>
        protected sealed override void RemoveItem(object itemContext)
        {
            base.RemoveItem(itemContext);

            this.queue.RemoveAll(item => item.ItemContext == itemContext);
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Update()
        {
            if (this.queue.Count == 0)
            {
                return;
            }

            this.timeRemaining -= Time.deltaTime;

            if (this.timeRemaining > 0)
            {
                return;
            }

            // Add next item to smoothed collection.
            var item = this.queue[0];
            this.queue.RemoveAt(0);

            base.CreateItem(item.ItemContext, item.ItemIndex);

            // Update timer.
            this.timeRemaining += this.Interval;
        }

        #endregion

        private class QueuedItem
        {
            #region Properties

            public object ItemContext { get; set; }

            public int ItemIndex { get; set; }

            #endregion
        }
    }
}