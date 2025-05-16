// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataDictionary.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    ///   Dictionary for data values that should be monitored and report changes to its listeners.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    public class DataDictionary<TKey, TValue> : DataDictionary, IDictionary<TKey, TValue>
    {
        #region Fields

        private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        /// <returns>
        ///   The number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int Count
        {
            get
            {
                return this.dictionary.Count;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> object has a fixed size.
        /// </summary>
        /// <returns>
        ///   true if the <see cref="T:System.Collections.IDictionary" /> object has a fixed size; otherwise, false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override bool IsFixedSize
        {
            get
            {
                return ((IDictionary)this.dictionary).IsFixedSize;
            }
        }

        /// <summary>
        ///   Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread
        ///   safe).
        /// </summary>
        /// <returns>
        ///   true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise,
        ///   false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override bool IsSynchronized
        {
            get
            {
                return ((ICollection)this.dictionary).IsSynchronized;
            }
        }

        /// <summary>
        ///   Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        ///   The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The property is set and the
        ///   <see cref="T:System.Collections.IDictionary" /> object is read-only.-or- The property is set, <paramref name="key" />
        ///   does not exist in the collection, and the <see cref="T:System.Collections.IDictionary" /> has a fixed size.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public override object this[object key]
        {
            get
            {
                return this[(TKey)key];
            }
            set
            {
                this[(TKey)key] = (TValue)value;
            }
        }

        /// <summary>
        ///   Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        ///   The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
        ///   The property is retrieved and
        ///   <paramref name="key" /> is not found.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The property is set and the
        ///   <see cref="T:System.Collections.Generic.IDictionary`2" /> is read-only.
        /// </exception>
        public TValue this[TKey key]
        {
            get
            {
                return this.dictionary[key];
            }
            set
            {
                TValue oldValue;
                if (this.dictionary.TryGetValue(key, out oldValue) && Equals(value, oldValue))
                {
                    return;
                }

                this.dictionary[key] = value;

                this.OnCollectionChanged();
            }
        }

        /// <summary>
        ///   Gets an <see cref="T:System.Collections.ICollection" /> object containing the keys of the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Collections.ICollection" /> object containing the keys of the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override ICollection Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        /// <summary>
        ///   Type of dictionary keys.
        /// </summary>
        public override Type KeyType
        {
            get
            {
                return typeof(TKey);
            }
        }

        /// <summary>
        ///   Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        /// <returns>
        ///   An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override object SyncRoot
        {
            get
            {
                return ((ICollection)this.dictionary).SyncRoot;
            }
        }

        /// <summary>
        ///   Gets an <see cref="T:System.Collections.ICollection" /> object containing the values in the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Collections.ICollection" /> object containing the values in the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override ICollection Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }

        /// <summary>
        ///   Type of dictionary values.
        /// </summary>
        public override Type ValueType
        {
            get
            {
                return typeof(TValue);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get
            {
                return this.dictionary.Keys;
            }
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get
            {
                return this.dictionary.Values;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds an element with the provided key and value to the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <param name="key">The <see cref="T:System.Object" /> to use as the key of the element to add. </param>
        /// <param name="value">The <see cref="T:System.Object" /> to use as the value of the element to add. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   An element with the same key already exists in the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.IDictionary" /> is read-only.-or-
        ///   The <see cref="T:System.Collections.IDictionary" /> has a fixed size.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public override void Add(object key, object value)
        {
            this.Add((TKey)key, (TValue)value);
        }

        /// <summary>
        ///   Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.Generic.ICollection`1" /> is
        ///   read-only.
        /// </exception>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        /// <summary>
        ///   Adds an element with the provided key and value to the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <param name="key">The object to use as the key of the element to add.</param>
        /// <param name="value">The object to use as the value of the element to add.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   An element with the same key already exists in the
        ///   <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.Generic.IDictionary`2" /> is
        ///   read-only.
        /// </exception>
        public void Add(TKey key, TValue value)
        {
            this.dictionary.Add(key, value);
            this.OnCollectionChanged();
        }

        /// <summary>
        ///   Removes all elements from the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.IDictionary" /> object is
        ///   read-only.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public override void Clear()
        {
            if (this.dictionary.Count == 0)
            {
                return;
            }

            this.dictionary.Clear();
            this.OnCollectionChanged();
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.IDictionary" /> object contains an element with the specified
        ///   key.
        /// </summary>
        /// <returns>
        ///   true if the <see cref="T:System.Collections.IDictionary" /> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" /> object.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <filterpriority>2</filterpriority>
        public override bool Contains(object key)
        {
            if (key is TKey)
            {
                return this.ContainsKey((TKey)key);
            }
            return false;
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
        /// </summary>
        /// <returns>
        ///   true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />;
        ///   otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the
        ///   specified key.
        /// </summary>
        /// <returns>
        ///   true if the <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with the key; otherwise,
        ///   false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.Generic.IDictionary`2" />.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return this.dictionary.ContainsKey(key);
        }

        /// <summary>
        ///   Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />,
        ///   starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///   The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
        ///   from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based
        ///   indexing.
        /// </param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" /> is multidimensional.-or- The number of elements
        ///   in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from
        ///   <paramref name="index" /> to the end of the destination <paramref name="array" />.-or- The type of the source
        ///   <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination
        ///   <paramref name="array." />
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public override void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
        ///   <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///   The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
        ///   from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have
        ///   zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex" /> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">
        ///   The number of elements in the source
        ///   <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from
        ///   <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
        /// </exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override IDictionaryEnumerator GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        /// <summary>
        ///   Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <param name="key">The key of the element to remove. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.IDictionary" /> object is
        ///   read-only.-or- The <see cref="T:System.Collections.IDictionary" /> has a fixed size.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public override void Remove(object key)
        {
            this.Remove((TKey)key);
        }

        /// <summary>
        ///   Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///   true if <paramref name="item" /> was successfully removed from the
        ///   <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
        ///   <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.Generic.ICollection`1" /> is
        ///   read-only.
        /// </exception>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Removes the element with the specified key from the <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </summary>
        /// <returns>
        ///   true if the element is successfully removed; otherwise, false.  This method also returns false if
        ///   <paramref name="key" /> was not found in the original <see cref="T:System.Collections.Generic.IDictionary`2" />.
        /// </returns>
        /// <param name="key">The key of the element to remove.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.Generic.IDictionary`2" /> is
        ///   read-only.
        /// </exception>
        public bool Remove(TKey key)
        {
            if (this.dictionary.Remove(key))
            {
                this.OnCollectionChanged();
                return true;
            }
            return false;
        }

        /// <summary>
        ///   Tries to get the values for the specified key.
        /// </summary>
        /// <param name="key">Key to get value for.</param>
        /// <param name="value">Value of specified key.</param>
        /// <returns>True if value for specified key could be found; otherwise, false.</returns>
        public override bool TryGetValue(object key, out object value)
        {
            if (key is TKey)
            {
                TValue specificValue;
                if (this.TryGetValue((TKey)key, out specificValue))
                {
                    value = specificValue;
                    return true;
                }
            }

            value = null;
            return false;
        }

        /// <summary>
        ///   Gets the value associated with the specified key.
        /// </summary>
        /// <returns>
        ///   true if the object that implements <see cref="T:System.Collections.Generic.IDictionary`2" /> contains an element with
        ///   the specified key; otherwise, false.
        /// </returns>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">
        ///   When this method returns, the value associated with the specified key, if the key is found;
        ///   otherwise, the default value for the type of the <paramref name="value" /> parameter. This parameter is passed
        ///   uninitialized.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.dictionary.TryGetValue(key, out value);
        }

        #endregion

        #region Methods

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return this.dictionary.GetEnumerator();
        }

        #endregion
    }

    /// <summary>
    ///   Dictionary for data values that should be monitored and report changes to its listeners.
    /// </summary>
    public abstract class DataDictionary : IDictionary
    {
        #region Delegates

        /// <summary>
        ///   Delegate for CollectionChanged event.
        /// </summary>
        public delegate void CollectionChangedDelegate();

        #endregion

        #region Events

        /// <summary>
        ///   Triggered when the dictionary changed.
        /// </summary>
        public event CollectionChangedDelegate CollectionChanged;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        /// <returns>
        ///   The number of elements contained in the <see cref="T:System.Collections.ICollection" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract int Count { get; }

        /// <summary>
        ///   Gets a value indicating whether the <see cref="T:System.Collections.IDictionary" /> object has a fixed size.
        /// </summary>
        /// <returns>
        ///   true if the <see cref="T:System.Collections.IDictionary" /> object has a fixed size; otherwise, false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract bool IsFixedSize { get; }

        /// <summary>
        ///   Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread
        ///   safe).
        /// </summary>
        /// <returns>
        ///   true if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise,
        ///   false.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract bool IsSynchronized { get; }

        /// <summary>
        ///   Gets or sets the element with the specified key.
        /// </summary>
        /// <returns>
        ///   The element with the specified key.
        /// </returns>
        /// <param name="key">The key of the element to get or set. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The property is set and the
        ///   <see cref="T:System.Collections.IDictionary" /> object is read-only.-or- The property is set, <paramref name="key" />
        ///   does not exist in the collection, and the <see cref="T:System.Collections.IDictionary" /> has a fixed size.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public abstract object this[object key] { get; set; }

        /// <summary>
        ///   Gets an <see cref="T:System.Collections.ICollection" /> object containing the keys of the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Collections.ICollection" /> object containing the keys of the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract ICollection Keys { get; }

        /// <summary>
        ///   Type of dictionary keys.
        /// </summary>
        public abstract Type KeyType { get; }

        /// <summary>
        ///   Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
        /// </summary>
        /// <returns>
        ///   An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract object SyncRoot { get; }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.ICollection"/> object containing the values in the <see cref="T:System.Collections.IDictionary"/> object.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.ICollection"/> object containing the values in the <see cref="T:System.Collections.IDictionary"/> object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract ICollection Values { get; }

        /// <summary>
        ///   Type of dictionary values.
        /// </summary>
        public abstract Type ValueType { get; }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Adds an element with the provided key and value to the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <param name="key">The <see cref="T:System.Object" /> to use as the key of the element to add. </param>
        /// <param name="value">The <see cref="T:System.Object" /> to use as the value of the element to add. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   An element with the same key already exists in the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.IDictionary" /> is read-only.-or-
        ///   The <see cref="T:System.Collections.IDictionary" /> has a fixed size.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public abstract void Add(object key, object value);

        /// <summary>
        ///   Removes all elements from the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.IDictionary" /> object is
        ///   read-only.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public abstract void Clear();

        /// <summary>
        ///   Determines whether the <see cref="T:System.Collections.IDictionary" /> object contains an element with the specified
        ///   key.
        /// </summary>
        /// <returns>
        ///   true if the <see cref="T:System.Collections.IDictionary" /> contains an element with the key; otherwise, false.
        /// </returns>
        /// <param name="key">The key to locate in the <see cref="T:System.Collections.IDictionary" /> object.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <filterpriority>2</filterpriority>
        public abstract bool Contains(object key);

        /// <summary>
        ///   Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />,
        ///   starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///   The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
        ///   from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based
        ///   indexing.
        /// </param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array" /> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index" /> is less than zero. </exception>
        /// <exception cref="T:System.ArgumentException">
        ///   <paramref name="array" /> is multidimensional.-or- The number of elements
        ///   in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from
        ///   <paramref name="index" /> to the end of the destination <paramref name="array" />.-or- The type of the source
        ///   <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination
        ///   <paramref name="array." />
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public abstract void CopyTo(Array array, int index);

        /// <summary>
        ///   Returns an <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Collections.IDictionaryEnumerator" /> object for the
        ///   <see cref="T:System.Collections.IDictionary" /> object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public abstract IDictionaryEnumerator GetEnumerator();

        /// <summary>
        ///   Removes the element with the specified key from the <see cref="T:System.Collections.IDictionary" /> object.
        /// </summary>
        /// <param name="key">The key of the element to remove. </param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key" /> is null. </exception>
        /// <exception cref="T:System.NotSupportedException">
        ///   The <see cref="T:System.Collections.IDictionary" /> object is
        ///   read-only.-or- The <see cref="T:System.Collections.IDictionary" /> has a fixed size.
        /// </exception>
        /// <filterpriority>2</filterpriority>
        public abstract void Remove(object key);

        /// <summary>
        ///   Tries to get the values for the specified key.
        /// </summary>
        /// <param name="key">Key to get value for.</param>
        /// <param name="value">Value of specified key.</param>
        /// <returns>True if value for specified key could be found; otherwise, false.</returns>
        public abstract bool TryGetValue(object key, out object value);

        #endregion

        #region Methods

        /// <summary>
        ///   Called when collection changed.
        /// </summary>
        protected virtual void OnCollectionChanged()
        {
            var handler = this.CollectionChanged;
            if (handler != null)
            {
                handler();
            }
        }

        /// <summary>
        ///   Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///   An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion
    }
}