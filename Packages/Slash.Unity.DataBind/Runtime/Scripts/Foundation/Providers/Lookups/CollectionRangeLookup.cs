// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionRangeLookup.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Lookups
{
    using System;
    using System.Linq;
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    public class CollectionRangeProvider : IDataProvider<Collection>
    {
        private Collection dataCollection;

        private int firstIndex;

        private int lastIndex;

        /// <summary>
        ///     Resulting collection which contains only the items from the specified range.
        /// </summary>
        private Collection<object> resultCollection;

        /// <inheritdoc />
        public event ValueChangedDelegate ValueChanged;

        public Collection DataCollection
        {
            get
            {
                return this.dataCollection;
            }
            set
            {
                if (value == this.dataCollection)
                {
                    return;
                }

                if (this.dataCollection != null)
                {
                    this.dataCollection.ItemAdded -= this.OnCollectionItemAdded;
                    this.dataCollection.ItemRemoved -= this.OnCollectionItemRemoved;
                    this.dataCollection.ItemInserted -= this.OnCollectionItemInserted;
                    this.dataCollection.ItemReplaced -= this.OnCollectionItemReplaced;
                    this.dataCollection.Cleared -= this.OnCollectionCleared;
                }

                this.dataCollection = value;

                if (this.dataCollection != null)
                {
                    this.dataCollection.ItemAdded += this.OnCollectionItemAdded;
                    this.dataCollection.ItemRemoved += this.OnCollectionItemRemoved;
                    this.dataCollection.ItemInserted += this.OnCollectionItemInserted;
                    this.dataCollection.ItemReplaced += this.OnCollectionItemReplaced;
                    this.dataCollection.Cleared += this.OnCollectionCleared;
                }

                if (this.dataCollection == null)
                {
                    this.ResultCollection = null;
                }
                else
                {
                    // Select value range.
                    var collection = new Collection<object>();

                    foreach (var item in this.dataCollection.Cast<object>().Skip(this.firstIndex)
                        .Take(this.lastIndex - this.firstIndex + 1))
                    {
                        collection.Add(item);
                    }

                    this.ResultCollection = collection;
                }
            }
        }

        /// <summary>
        ///     Index of first item to include in result collection.
        /// </summary>
        public int FirstIndex
        {
            get
            {
                return this.firstIndex;
            }
            set
            {
                if (value == this.firstIndex)
                {
                    return;
                }

                var oldFirstIndex = this.firstIndex;
                this.firstIndex = value;

                if (this.resultCollection != null)
                {
                    if (oldFirstIndex < this.firstIndex)
                    {
                        var itemsToRemove = this.firstIndex - oldFirstIndex;
                        for (var i = 0; i < itemsToRemove; i++)
                        {
                            if (this.resultCollection.Count > 0)
                            {
                                this.resultCollection.RemoveAt(0);
                            }
                        }
                    }
                    else
                    {
                        for (var i = this.firstIndex; i < oldFirstIndex; i++)
                        {
                            if (i >= 0 && i <= this.lastIndex &&
                                i < this.dataCollection.Count)
                            {
                                this.resultCollection.Add(this.dataCollection[i]);
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public bool IsInitialized
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        ///     Index of last item to include in result collection.
        /// </summary>
        public int LastIndex
        {
            get
            {
                return this.lastIndex;
            }
            set
            {
                if (value == this.lastIndex)
                {
                    return;
                }

                var oldLastIndex = this.lastIndex;
                this.lastIndex = value;

                if (this.resultCollection != null)
                {
                    if (oldLastIndex < this.lastIndex)
                    {
                        for (var i = oldLastIndex + 1; i <= this.lastIndex; i++)
                        {
                            if (i >= this.firstIndex && i < this.dataCollection.Count)
                            {
                                this.resultCollection.Add(this.dataCollection[i]);
                            }
                        }
                    }
                    else
                    {
                        var expectedCount = Math.Max(0, this.lastIndex - this.firstIndex + 1);
                        while (this.resultCollection.Count > expectedCount)
                        {
                            this.resultCollection.RemoveAt(this.resultCollection.Count - 1);
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public Collection Value
        {
            get
            {
                return this.resultCollection;
            }
        }

        /// <summary>
        ///     Resulting collection which contains only the items from the specified range.
        /// </summary>
        private Collection<object> ResultCollection
        {
            set
            {
                if (value == this.resultCollection)
                {
                    return;
                }

                this.resultCollection = value;

                this.OnValueChanged();
            }
        }

        /// <inheritdoc />
        object IDataProvider.Value
        {
            get
            {
                return this.Value;
            }
        }

        protected virtual void OnValueChanged()
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler();
            }
        }

        private void OnCollectionCleared()
        {
            this.ResultCollection = null;
        }

        private void OnCollectionItemAdded(object item)
        {
            // Check if item has to be added to result collection.
            if (this.dataCollection.Count - 1 <= this.lastIndex)
            {
                this.resultCollection.Add(item);
            }
        }

        private void OnCollectionItemInserted(object item, int index)
        {
            if (this.firstIndex <= index && index < this.lastIndex)
            {
                // Insert in result collection and remove last item.
                var resultIndex = index - this.firstIndex;
                this.resultCollection.Insert(resultIndex, item);
                this.resultCollection.RemoveAt(this.resultCollection.Count - 1);
            }
        }

        private void OnCollectionItemRemoved(object item)
        {
            // Check if item was part of result collection.
            if (this.resultCollection.Remove(item))
            {
                // Add new item.
                if (this.lastIndex < this.dataCollection.Count)
                {
                    this.resultCollection.Add(this.dataCollection[this.lastIndex]);
                }
            }
        }

        private void OnCollectionItemReplaced(int index, object previousItem, object newItem)
        {
            if (this.firstIndex <= index && index < this.lastIndex)
            {
                // Replace in result collection.
                var resultIndex = index - this.firstIndex;
                this.resultCollection[resultIndex] = newItem;
            }
        }
    }

    /// <summary>
    ///     Returns a part of a given collection.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Lookups/[DB] Collection Range Lookup")]
    public class CollectionRangeLookup : DataProvider
    {
        /// <summary>
        ///     Collection to get the items from.
        /// </summary>
        [Tooltip("Collection to get item from.")]
        public DataBinding Collection;

        /// <summary>
        ///     Index of the first item to get from the collection.
        /// </summary>
        [Tooltip("Index of the first item to get from the collection.")]
        public DataBinding FirstIndex;

        /// <summary>
        ///     Index of the last item to get from the collection.
        /// </summary>
        [Tooltip("Index of the last item to get from the collection.")]
        public DataBinding LastIndex;

        private CollectionRangeProvider collectionRangeProvider;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return this.collectionRangeProvider != null ? this.collectionRangeProvider.Value : null;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.FirstIndex);
            this.RemoveBinding(this.LastIndex);
            this.RemoveBinding(this.Collection);
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();
            this.Collection.ValueChanged -= this.OnDataCollectionChanged;
            this.FirstIndex.ValueChanged -= this.OnFirstIndexChanged;
            this.LastIndex.ValueChanged -= this.OnLastIndexChanged;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            this.Collection.ValueChanged += this.OnDataCollectionChanged;
            this.FirstIndex.ValueChanged += this.OnFirstIndexChanged;
            this.LastIndex.ValueChanged += this.OnLastIndexChanged;

            // Init values.
            this.OnDataCollectionChanged();
            this.OnFirstIndexChanged();
            this.OnLastIndexChanged();
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.collectionRangeProvider = new CollectionRangeProvider();
            this.collectionRangeProvider.ValueChanged += this.OnValueChanged;

            this.AddBinding(this.FirstIndex);
            this.AddBinding(this.LastIndex);
            this.AddBinding(this.Collection);
        }

        private void OnDataCollectionChanged()
        {
            this.collectionRangeProvider.DataCollection = this.Collection.GetValue<Collection>();
        }

        private void OnFirstIndexChanged()
        {
            this.collectionRangeProvider.FirstIndex = this.FirstIndex.GetValue<int>();
        }

        private void OnLastIndexChanged()
        {
            this.collectionRangeProvider.LastIndex = this.LastIndex.GetValue<int>();
        }
    }
}