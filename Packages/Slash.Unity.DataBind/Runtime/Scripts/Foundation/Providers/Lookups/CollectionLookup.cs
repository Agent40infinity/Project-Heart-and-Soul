// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionLookup.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Lookups
{
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Looks up an item with a specific index from a given collection.
    /// </summary>
    public class CollectionLookup : DataProvider
    {
        /// <summary>
        ///     Collection to get item from.
        /// </summary>
        [Tooltip("Collection to get item from.")]
        public DataBinding Collection;

        private Collection dataCollection;

        /// <summary>
        ///     Default value if index wasn't found in collection.
        /// </summary>
        [Tooltip("Default value if index wasn't found in collection.")]
        public string DefaultValue;

        /// <summary>
        ///     Index of item to get from collection.
        /// </summary>
        [Tooltip("Index of item to get from collection.")]
        public DataBinding Index;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                object value = string.IsNullOrEmpty(this.DefaultValue) ? null : this.DefaultValue;
                if (this.DataCollection != null)
                {
                    var index = this.Index.GetValue<int>();
                    foreach (var dataValue in this.DataCollection)
                    {
                        if (index == 0)
                        {
                            value = dataValue;
                            break;
                        }
                        --index;
                    }
                }

                return value;
            }
        }

        private Collection DataCollection
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
                    this.dataCollection.Cleared -= this.OnCollectionCleared;
                }

                this.dataCollection = value;

                if (this.dataCollection != null)
                {
                    this.dataCollection.ItemAdded += this.OnCollectionItemAdded;
                    this.dataCollection.ItemRemoved += this.OnCollectionItemRemoved;
                    this.dataCollection.Cleared += this.OnCollectionCleared;
                }

                this.UpdateValue();
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Index);
            this.RemoveBinding(this.Collection);
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();
            this.Collection.ValueChanged -= this.OnDataCollectionChanged;
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            this.Collection.ValueChanged += this.OnDataCollectionChanged;
            this.DataCollection = this.Collection.GetValue<Collection>();
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Index);
            this.AddBinding(this.Collection);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }

        private void OnCollectionCleared()
        {
            this.UpdateValue();
        }

        private void OnCollectionItemAdded(object item)
        {
            this.UpdateValue();
        }

        private void OnCollectionItemRemoved(object item)
        {
            this.UpdateValue();
        }

        private void OnDataCollectionChanged()
        {
            this.DataCollection = this.Collection.GetValue<Collection>();
        }
    }
}