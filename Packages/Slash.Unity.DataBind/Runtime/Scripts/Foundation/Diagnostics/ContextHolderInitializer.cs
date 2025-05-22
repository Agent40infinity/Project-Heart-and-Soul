// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContextHolderInitializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Foundation.Diagnostics
{
    using System;

    using Slash.Unity.DataBind.Core.Presentation;
    using Slash.Unity.DataBind.Core.Utils;

    using UnityEngine;

    /// <summary>
    ///   Initializes a context holder on start. Useful e.g. for debugging if the context holder
    ///   should only be initialized directly for testing and will be initialized otherwise via code.
    /// </summary>
    public class ContextHolderInitializer : MonoBehaviour
    {
        #region Fields

        /// <summary>
        ///   Context holder to initialize.
        /// </summary>
        [Tooltip("Context holder to initialize.")]
        public ContextHolder ContextHolder;

        [SerializeField]
        [ContextType]
        private string contextType;

        #endregion

        #region Properties

        /// <summary>
        ///   Type of context to create on startup.
        /// </summary>
        public Type ContextType
        {
            get
            {
                try
                {
                    return this.contextType != null ? ReflectionUtils.FindType(this.contextType) : null;
                }
                catch (TypeLoadException)
                {
                    Debug.LogError("Can't find context type '" + this.contextType + "'.", this);
                    return null;
                }
            }
            set
            {
                this.contextType = value != null ? value.AssemblyQualifiedName : null;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Reset()
        {
            if (this.ContextHolder == null)
            {
                this.ContextHolder = this.GetComponent<ContextHolder>();
            }
        }

        /// <summary>
        ///   Unity callback.
        /// </summary>
        protected void Start()
        {
            if (this.ContextHolder != null && this.ContextType != null)
            {
                if (this.ContextHolder.Context == null)
                {
                    this.ContextHolder.SetContext(Activator.CreateInstance(this.ContextType), null);
                }
            }
        }

        #endregion
    }
}