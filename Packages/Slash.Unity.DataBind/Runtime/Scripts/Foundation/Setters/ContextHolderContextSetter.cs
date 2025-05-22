// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextHolderContextSetter.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Setters
{
    using Slash.Unity.DataBind.Core.Presentation;

    using UnityEngine;

    /// <summary>
    ///   Sets the context of a specific context holder.
    /// </summary>
    [AddComponentMenu("Data Bind/Foundation/Setters/[DB] Context Holder Context Setter")]
    public class ContextHolderContextSetter : ComponentSingleSetter<ContextHolder, object>
    {
        /// <inheritdoc />
        protected override void UpdateTargetValue(ContextHolder target, object value)
        {
            var path = this.Data.Type == DataBindingType.Context ? this.Data.Path : null;
            target.SetContext(value, path);
        }
    }
}