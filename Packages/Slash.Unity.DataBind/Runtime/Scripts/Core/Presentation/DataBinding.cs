// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBinding.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;
    using Object = UnityEngine.Object;

	/// <summary>
	///     Structure which hold the information to which data to bind.
	/// </summary>
	[Serializable]
    public sealed class DataBinding : IDataProvider
    {
        /// <summary>
        ///     Constant value.
        /// </summary>
        public string Constant;

        /// <summary>
        ///     Path to value in data context.
        /// </summary>
        [ContextPath(Filter = ~ContextMemberFilter.Methods)]
        public string Path;

        /// <summary>
        ///     Referenced data provider.
        /// </summary>
        public DataProvider Provider;

        /// <summary>
        ///     Unity object reference.
        /// </summary>
        public Object Reference;

        /// <summary>
        ///     Type of data binding.
        /// </summary>
        public DataBindingType Type;

        /// <summary>
        ///     Node to get the data from a context.
        /// </summary>
        private DataContextNodeConnector dataContextNodeConnector;

        private IDataProvider provider;

        /// <summary>
        ///     Current data value.
        /// </summary>
        private object value;

        /// <summary>
        ///     Returns the name of the binding to use for debugging.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Binding has an unknown type.</exception>
        public string DebugName
        {
            get
            {
                string bindingIdentifier;
                switch (this.Type)
                {
                    case DataBindingType.Context:
                        bindingIdentifier = this.Path;
                        break;
                    case DataBindingType.Provider:
                        bindingIdentifier = this.provider != null ? this.provider.ToString() : "null";
                        break;
                    case DataBindingType.Constant:
                        bindingIdentifier = this.Constant;
                        break;
                    case DataBindingType.Reference:
                        bindingIdentifier = this.Reference != null ? this.Reference.name : "null";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return string.Format("{0}: {1}", this.Type, bindingIdentifier);
            }
        }

        /// <summary>
        ///     Called when the data value changed.
        /// </summary>
        public event ValueChangedDelegate ValueChanged;

        /// <summary>
        ///     Indicates if the data binding already holds a valid value.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Current data value.
        /// </summary>
        public object Value
        {
            get { return this.value; }
            private set
            {
                if (Equals(value, this.value))
                {
                    return;
                }

                this.value = value;
                this.OnValueChanged();
            }
        }

        /// <summary>
        ///     Deinitializes the data binding, e.g. unregistering from events.
        /// </summary>
        public void Deinit()
        {
            if (this.provider != null)
            {
                this.provider.ValueChanged -= this.OnProviderValueChanged;
            }

            if (this.dataContextNodeConnector != null)
            {
                this.dataContextNodeConnector.SetValueListener(null);
                this.dataContextNodeConnector = null;
            }
        }

        /// <summary>
        ///     Returns the current data value, converted to the specified type.
        /// </summary>
        /// <typeparam name="T">Desired type of data.</typeparam>
        /// <returns>Current data value, converted to the specified type.</returns>
        /// <exception cref="System.InvalidCastException">Thrown if the data value can't be cast to the specified type.</exception>
        public T GetValue<T>()
        {
            try
            {
                return (T) this.GetValue(typeof(T));
            }
            catch (InvalidCastException)
            {
                throw new InvalidCastException(
                    string.Format(
                        "Can't cast value '{0}' of binding '{1}' to type '{2}'",
                        this.Value,
                        this.DebugName,
                        typeof(T)));
            }
        }

        /// <summary>
        ///     Returns the current data value, converted to the specified type.
        /// </summary>
        /// <param name="type">Desired type of data.</param>
        /// <returns>Current data value, converted to the specified type.</returns>
        /// <exception cref="System.InvalidCastException">Thrown if the data value can't be cast to the specified type.</exception>
        public object GetValue(Type type)
        {
            var rawValue = this.Value;
            if (rawValue == null)
            {
                return TypeInfoUtils.IsValueType(type) ? Activator.CreateInstance(type) : null;
            }

            object convertedValue;
            return ReflectionUtils.TryConvertValue(rawValue, type, out convertedValue) ? convertedValue : rawValue;
        }

        /// <summary>
        ///     Initializes the data binding, depending on the type of data binding.
        /// </summary>
        /// <param name="dataContextNodeConnectorInitializer">Initializer for data context node connectors.</param>
        /// <param name="monoBehaviour">Mono behaviour this data binding belongs to.</param>
        public void Init(DataContextNodeConnectorInitializer dataContextNodeConnectorInitializer, MonoBehaviour monoBehaviour)
        {
            if (this.IsInitialized)
            {
                Debug.LogWarning("Data Binding is already initialized", monoBehaviour);
                return;
            }

            switch (this.Type)
            {
                case DataBindingType.Context:
                {
                    this.dataContextNodeConnector = new DataContextNodeConnector(dataContextNodeConnectorInitializer, monoBehaviour, this.Path);
                    var initialValue = this.dataContextNodeConnector.SetValueListener(this.OnTargetValueChanged);
                    if (this.dataContextNodeConnector.IsInitialized)
                    {
                        this.OnTargetValueChanged(initialValue);
                    }
                }
                    break;
                case DataBindingType.Provider:
                {
                    this.InitProvider(this.Provider);
                }
                    break;
                case DataBindingType.Constant:
                {
                    this.OnTargetValueChanged(this.Constant);
                }
                    break;
                case DataBindingType.Reference:
                {
                    this.OnTargetValueChanged(this.Reference == null ? null : this.Reference);
                }
                    break;
            }
        }

        /// <summary>
        ///     Initializes the data provider of this binding.
        /// </summary>
        /// <param name="dataProvider">Data provider this binding should use.</param>
        public void InitProvider(IDataProvider dataProvider)
        {
            this.provider = dataProvider;
            if (dataProvider != null)
            {
                dataProvider.ValueChanged += this.OnProviderValueChanged;
                if (dataProvider.IsInitialized)
                {
                    this.OnTargetValueChanged(dataProvider.Value);
                }
            }
            else
            {
                this.OnTargetValueChanged(null);
            }
        }

        /// <summary>
        ///     Has to be called when an anchestor context changed as the data value may change.
        /// </summary>
        public void OnContextChanged()
        {
            if (this.dataContextNodeConnector != null)
            {
                this.dataContextNodeConnector.OnHierarchyChanged();
            }
        }

        private void OnProviderValueChanged()
        {
            this.OnTargetValueChanged(this.provider.Value);
        }

        private void OnTargetValueChanged(object newValue)
        {
            if (this.IsInitialized)
            {
                // Only trigger callback if value changed.
                this.Value = newValue;
            }
            else
            {
                this.IsInitialized = true;

                // On initialization make sure the callback is triggered.
                this.value = newValue;
                this.OnValueChanged();
            }
        }

        private void OnValueChanged()
        {
            var handler = this.ValueChanged;
            if (handler != null)
            {
                handler();
            }
        }
    }
}