// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FindGameObjectWithTagGetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Providers.Getters
{
    using Slash.Unity.DataBind.Core.Presentation;
    using UnityEngine;

    /// <summary>
    ///     Returns the game object with the specific tag or null if none was found.
    /// </summary>
    public class FindGameObjectWithTagGetter : DataProvider
    {
        /// <summary>
        ///     Tag to find.
        /// </summary>
        public DataBinding Tag;

        /// <inheritdoc />
        public override object Value
        {
            get
            {
                var tagToFind = this.Tag.GetValue<string>();
                if (string.IsNullOrEmpty(tagToFind))
                {
                    return null;
                }
                try
                {
                    return GameObject.FindWithTag(tagToFind);
                }
                catch (UnityException)
                {
                    return null;
                }
            }
        }

        /// <inheritdoc />
        public override void Deinit()
        {
            this.RemoveBinding(this.Tag);
        }

        /// <inheritdoc />
        public override void Init()
        {
            this.AddBinding(this.Tag);
        }

        /// <inheritdoc />
        protected override void UpdateValue()
        {
            this.OnValueChanged();
        }
    }
}