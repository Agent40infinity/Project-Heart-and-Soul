// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionWhereBehaviour.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Lookups
{
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    /// <summary>
    ///     Returns the item of the collection that has a specific value at a specific path.
    /// </summary>
    public class CollectionWhereBehaviour : DataProvider
    {
        /// <summary>
        ///     Collection to check items.
        /// </summary>
        public DataBinding Collection;

        /// <summary>
        ///     Value to compare item value with.
        /// </summary>
        public DataBinding ComparisonValue;

        private Collection dataCollection;

        /// <summary>
        ///     Path to value within item to compare to comparison value.
        /// </summary>
        [ContextPath(PathDisplayName = "Item Path")]
        public string ItemPath;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var collection = this.Collection.GetValue<Collection>();
                if (collection == null)
                {
                    return null;
                }

                var comparisonValue = this.ComparisonValue.Value;
                foreach (var item in collection)
                {
                    var itemValue = item;
                    var itemContext = item as IDataContext;
                    if (itemContext != null && !string.IsNullOrEmpty(this.ItemPath))
                    {
                        itemValue = itemContext.GetValue(this.ItemPath);
                    }

                    if (ComparisonUtils.CheckValuesForEquality(itemValue, comparisonValue))
                    {
                        return item;
                    }
                }
                return null;
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
            this.RemoveBinding(this.Collection);
            this.RemoveBinding(this.ComparisonValue);
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
            this.AddBinding(this.Collection);
            this.AddBinding(this.ComparisonValue);
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