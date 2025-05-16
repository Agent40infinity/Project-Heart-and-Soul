// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionItemsConverter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Converters
{
    using System;

    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;

    /// <summary>
    ///   Data provider that converts the items of a collection and returns the converted collection.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Providers/Converters/[DB] Collection Items Converter")]
    public class CollectionItemsConverter : DataProvider
    {
        /// <summary>
        ///   Collection to convert.
        /// </summary>
        public DataBinding Collection;

        /// <summary>
        ///   Type of converter to use.
        /// </summary>
        [TypeSelection(BaseType = typeof(ValueConverter))]
        public string ConverterType;

        private CollectionDataBinding<object> collectionBinding;

        private Collection<object> convertedItems;

        private ValueConverter valueConverter;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                return this.convertedItems;
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            base.Deinit();

            this.RemoveBinding(this.Collection);

            this.valueConverter = null;

            this.collectionBinding = null;
        }

        /// <inheritdoc />
        public override void Disable()
        {
            base.Disable();

            this.collectionBinding.Disable();
        }

        /// <inheritdoc />
        public override void Enable()
        {
            base.Enable();

            this.collectionBinding.Enable();
        }

        /// <inheritdoc />
        public override void Init()
        {
            base.Init();

            this.AddBinding(this.Collection);

            if (!string.IsNullOrEmpty(this.ConverterType))
            {
                // Create data converter.
                this.valueConverter =
                    (ValueConverter)Activator.CreateInstance(ReflectionUtils.FindType(this.ConverterType));
            }
            else
            {
                Debug.LogWarning("No converter set", this);
            }

            // Observe collection.
            this.collectionBinding = new CollectionDataBinding<object>(this.Collection)
            {
                CollectionChanged = this.OnCollectionChanged,
                ItemAdded = this.OnItemAdded,
                ItemInserted = this.OnItemInserted,
                ItemRemoved = this.OnItemRemoved
            };
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
        }

        private void OnCollectionChanged(Collection collection)
        {
            this.convertedItems = new Collection<object>();
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    this.convertedItems.Add(this.valueConverter.Convert(item));
                }
            }
            this.OnValueChanged();
        }

        private void OnItemAdded(object item)
        {
            this.convertedItems.Add(this.valueConverter.Convert(item));
        }

        private void OnItemInserted(int index, object item)
        {
            this.convertedItems.Insert(index, this.valueConverter.Convert(item));
        }

        private void OnItemRemoved(object obj)
        {
            // TODO: Remove converted item, but how?
        }
    }
}