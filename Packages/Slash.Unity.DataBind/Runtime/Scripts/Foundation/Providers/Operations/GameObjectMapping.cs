// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GameObjectMapping.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Pair of a string key and a game object value.
    /// </summary>
    [Serializable]
    public class StringGameObjectPair
    {
        /// <summary>
        ///     Key of pair.
        /// </summary>
        public string Key;

        /// <summary>
        ///     Value of pair.
        /// </summary>
        public GameObject Value;
    }

    /// <summary>
    ///     Maps a string key on to a game object.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Operations/[DB] Game Object Mapping")]
    public class GameObjectMapping : DataProvider
    {
        /// <summary>
        ///     Default value to use if no mapping found.
        /// </summary>
        public GameObject Default;

        /// <summary>
        ///     Key to do mapping with.
        /// </summary>
        [Tooltip("Key to do mapping with.")]
        public DataBinding Key;

        /// <summary>
        ///     Mappings between string keys and game object values.
        /// </summary>
        public List<StringGameObjectPair> Mapping;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var key = this.Key.GetValue<string>();
                if (string.IsNullOrEmpty(key))
                {
                    return this.Default;
                }
                var pair = this.Mapping.FirstOrDefault(existingPair => existingPair.Key == key);
                return pair != null ? pair.Value : this.Default;
            }
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Key);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}