// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataNode.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
    using System;
    using System.ComponentModel;
    using Slash.Unity.DataBind.Core.Utils;

    /// <summary>
    ///     Wraps a data object in the context tree and makes sure that the registered listeners are informed
    ///     when the data value changed.
    /// </summary>
    public abstract class DataNode : IDataNode
    {
        /// <summary>
        ///     Name of node.
        /// </summary>
        private readonly string name;

        /// <summary>
        ///     Parent node.
        /// </summary>
        private readonly IDataNode parentNode;

        /// <summary>
        ///     Cached type information of the data value this node capsules.
        /// </summary>
        private readonly NodeTypeInfo typeInfo;

        /// <summary>
        ///     Observes the value of the node and informs when it changed.
        /// </summary>
        private INodeValueObserver nodeValueObserver;

        /// <summary>
        ///     Current node value.
        /// </summary>
        private object value;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="typeInfo">Type info of node.</param>
        /// <param name="parentNode">Parent node.</param>
        /// <param name="name">Name of node. Usually the name of the property/field this node represents.</param>
        protected DataNode(NodeTypeInfo typeInfo, IDataNode parentNode, string name)
        {
            this.typeInfo = typeInfo;
            this.name = name;
            this.parentNode = parentNode;

            if (parentNode != null)
            {
                parentNode.ValueChanged += this.OnParentValueChanged;
            }

            var parentValue = parentNode != null ? parentNode.Value : null;
            if (parentValue != null)
            {
                // Create node value observer.
                this.NodeValueObserver = this.CreateNodeValueObserver(parentValue);
            }

            // Get initial value for the node.
            this.SetCachedValue(this.typeInfo.GetValue(parentValue));
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="typeInfo">Type info of node.</param>
        /// <param name="name">Name of node.</param>
        protected DataNode(NodeTypeInfo typeInfo, string name)
        {
            this.typeInfo = typeInfo;
            this.name = name;

            this.SetCachedValue(this.typeInfo.GetValue(null));
        }

        /// <inheritdoc />
        public event Action<object> ValueChanged;

        /// <inheritdoc />
        public Type DataType
        {
            get
            {
                return this.typeInfo.Type;
            }
        }

        /// <inheritdoc />
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <inheritdoc />
        public object Value
        {
            get
            {
                return this.value;
            }
        }

        private INodeValueObserver NodeValueObserver
        {
            set
            {
                if (value == this.nodeValueObserver)
                {
                    return;
                }

                if (this.nodeValueObserver != null)
                {
                    this.nodeValueObserver.ValueChanged -= this.OnNodeValueChanged;
                }

                this.nodeValueObserver = value;

                if (this.nodeValueObserver != null)
                {
                    this.nodeValueObserver.ValueChanged += this.OnNodeValueChanged;
                }
            }
        }

        /// <inheritdoc />
        public abstract IDataNode FindDescendant(string path);

        /// <inheritdoc />
        public void SetValue(object newValue)
        {
            if (this.parentNode != null)
            {
                var parentValue = this.parentNode.Value;

                // Try to cast value if not already target type.
                if (newValue != null && newValue.GetType() != this.typeInfo.Type)
                {
                    object convertedValue;
                    if (!ReflectionUtils.TryConvertValue(newValue, this.typeInfo.Type, out convertedValue))
                    {
                        throw new InvalidCastException(string.Format(
                            "Can't cast new value '{0}' (Type: '{1}') to expected type '{2}'", newValue,
                            newValue.GetType(), this.typeInfo.Type));
                    }
                    newValue = convertedValue;
                }

                // Update data value.
                this.typeInfo.SetValue(parentValue, newValue);

                // Update parent node if parent is a value type.
                if (parentValue != null &&
                    parentValue.GetType().IsValueType)
                {
                    this.parentNode.SetValue(parentValue);
                }
            }

            this.SetCachedValue(newValue);
        }

        /// <summary>
        ///     Invokes the ValueChanged event.
        /// </summary>
        /// <param name="newValue">New value.</param>
        protected void OnValueChanged(object newValue)
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler(newValue);
            }
        }

        private INodeValueObserver CreateNodeValueObserver(object obj)
        {
            // Skip for nodes which can't change their values.
            if (!this.typeInfo.CanValueChange)
            {
                return null;
            }

            // Check parent value.
            var notifyPropertyChangedParent = obj as INotifyPropertyChanged;
            if (notifyPropertyChangedParent != null)
            {
                return new NotifyPropertyChangedNodeValueObserver(notifyPropertyChangedParent, this.name);
            }

            // Try to get data provider as node value observer.
            var dataProvider = DataBindingSettingsProvider.GetDataProvider(obj, this.name);
            if (dataProvider != null)
            {
                return new DataProviderNodeValueObserver(dataProvider);
            }

            var settings = DataBindingSettingsProvider.Settings;
            if (settings.ReportMissingBackingFields)
            {
                settings.LogWarning(string.Format("No backing field found for property '{0}' of type '{1}'",
                    this.name, obj != null ? obj.GetType().Name : "null"));
            }

            return null;
        }

        private void OnCollectionCleared()
        {
            this.OnValueChanged(this.value);
        }

        private void OnCollectionItemAdded(object item)
        {
            this.OnValueChanged(this.value);
        }

        private void OnCollectionItemRemoved(object item)
        {
            this.OnValueChanged(this.value);
        }

        private void OnNodeValueChanged()
        {
            // Update cached value.
            this.SetCachedValue(this.typeInfo.GetValue(this.parentNode != null ? this.parentNode.Value : null));
        }

        private void OnParentValueChanged(object newParentValue)
        {
            // Update node value observer.
            this.NodeValueObserver = this.CreateNodeValueObserver(newParentValue);

            // Get object of the node.
            this.SetCachedValue(this.typeInfo.GetValue(newParentValue));
        }

        private void SetCachedValue(object newValue)
        {
            // Update cached value.
            if (this.value != null && newValue != null && this.value.Equals(newValue))
            {
                return;
            }

            // Handle specific changed events for collection.
            var collectionValue = this.value as Collection;
            if (collectionValue != null)
            {
                collectionValue.Cleared -= this.OnCollectionCleared;
                collectionValue.ItemAdded -= this.OnCollectionItemAdded;
                collectionValue.ItemRemoved -= this.OnCollectionItemRemoved;
            }

            this.value = newValue;

            // Handle specific changed events for collection.
            collectionValue = this.value as Collection;
            if (collectionValue != null)
            {
                collectionValue.Cleared += this.OnCollectionCleared;
                collectionValue.ItemAdded += this.OnCollectionItemAdded;
                collectionValue.ItemRemoved += this.OnCollectionItemRemoved;
            }

            this.OnValueChanged(this.value);
        }
    }
}