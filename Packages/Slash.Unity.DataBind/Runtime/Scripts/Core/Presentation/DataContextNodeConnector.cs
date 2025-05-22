// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContextNodeConnector.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Slash.Unity.DataBind.Core.Data;
    using Slash.Unity.DataBind.Core.Utils;
    using UnityEngine;

    /// <summary>
    ///     Node which works with a data context and caches contexts and master paths.
    ///     Always bound to a specific game object which specifies the hierarchy.
    /// </summary>
    public sealed class DataContextNodeConnector
    {
        private const int MaxPathDepth = 100500;

        /// <summary>
        ///     Context cache for faster look up.
        /// </summary>
        private readonly Dictionary<int, ContextHolder> contexts = new Dictionary<int, ContextHolder>();

        /// <summary>
        ///     Initializer that takes care about the initialization of this connector.
        /// </summary>
        private readonly DataContextNodeConnectorInitializer initializer;

        /// <summary>
        ///     Master path cache for faster look up.
        /// </summary>
        private readonly Dictionary<int, string> masterPaths = new Dictionary<int, string>();

        /// <summary>
        ///     Mono behaviour to do the lookup for.
        /// </summary>
        private readonly MonoBehaviour monoBehaviour;

        /// <summary>
        ///     Path in context this node is bound to.
        /// </summary>
        private readonly string path;

        /// <summary>
        ///     Context to use for data lookup.
        /// </summary>
        private object context;

        /// <summary>
        ///     Full path to data starting from context.
        /// </summary>
        private string contextPath;

        /// <summary>
        ///     Callback when value changed.
        /// </summary>
        private Action<object> valueChangedCallback;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="initializer">Initializer that takes care about the initialization of this connector.</param>
        /// <param name="monoBehaviour">Mono behaviour this node is assigned to.</param>
        /// <param name="path">Path in context this node is bound to.</param>
        public DataContextNodeConnector(DataContextNodeConnectorInitializer initializer, MonoBehaviour monoBehaviour,
            string path)
        {
            if (initializer == null)
            {
                throw new ArgumentNullException("initializer", "No initializer provided");
            }
            if (monoBehaviour == null)
            {
                throw new ArgumentNullException("monoBehaviour", "No mono behaviour provided");
            }
            
            this.initializer = initializer;
            this.monoBehaviour = monoBehaviour;
            this.path = path;
            this.OnHierarchyChanged();
        }

        /// <summary>
        ///     Current context for this node.
        /// </summary>
        public object Context
        {
            get
            {
                return this.context;
            }
            private set
            {
                if (value == this.context)
                {
                    return;
                }

                // Remove listener from old context.
                this.RemoveListener();
                this.IsInitialized = false;
                
                this.context = value;

                // Add listener to new context.
                var initialValue = this.RegisterListener();

                // There might be additional data context nodes that aren't updated yet, so delay intial value change until end of frame.
                if (this.monoBehaviour.isActiveAndEnabled)
                {
                    this.initializer.RegisterInitialization(this, initialValue);
                }
                else
                {
                    this.IsInitialized = true;
                    this.OnValueChanged(initialValue);
                }
            }
        }

        /// <summary>
        ///     Indicates if the context node already holds a valid value.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        ///     Initializes this connector.
        /// </summary>
        /// <param name="initialValue">Initial value.</param>
        public void Init(object initialValue)
        {
            if (this.IsInitialized)
            {
                throw new InvalidOperationException("Connector is already initialized");
            }

            this.IsInitialized = true;
            this.OnValueChanged(initialValue);
        }

        /// <summary>
        ///     Informs the context node that the hierarchy changed, so the context and/or master paths may have changed.
        ///     Has to be called:
        ///     - Anchestor context changed.
        ///     - Anchestor master path changed.
        /// </summary>
        public void OnHierarchyChanged()
        {
            // Update master paths.
            this.UpdateCache();

            // Update context.
            var depthToGo = GetPathDepth(this.path);

            // Take first context holder which is deep enough to use as a starting point.
            var contextHolderPair = depthToGo == MaxPathDepth
                ? this.contexts.OrderByDescending(pair => pair.Key).FirstOrDefault()
                : this.contexts.FirstOrDefault(pair => depthToGo <= pair.Key);
            var contextHolder = contextHolderPair.Value;

            var newContext = contextHolder != null ? contextHolder.Context : null;

            // Adjust full path.
            this.contextPath = this.GetFullCleanPath(depthToGo, contextHolderPair.Key);

            this.Context = newContext;
        }

        /// <summary>
        ///     Sets the specified value at the specified path.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(object value)
        {
            // Set value on data context.
            var dataContext = this.context as IDataContext;
            if (dataContext == null)
            {
                return;
            }

            // Make sure connector is initialized.
            if (!this.IsInitialized)
            {
                throw new InvalidOperationException("DataContextNodeConnector not initialized yet");
            }

            try
            {
                dataContext.SetValue(this.contextPath, value);
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e, this.monoBehaviour);
            }
            catch (InvalidOperationException e)
            {
                Debug.LogError(e, this.monoBehaviour);
            }
        }

        /// <summary>
        ///     Sets the callback which is called when the value of the monitored data in the context changed.
        /// </summary>
        /// <param name="onValueChanged">Callback to invoke when the value of the monitored data in the context changed.</param>
        /// <returns>Initial value.</returns>
        public object SetValueListener(Action<object> onValueChanged)
        {
            // Remove old callback.
            this.RemoveListener();

            this.valueChangedCallback = onValueChanged;

            // Add new callback.
            return this.RegisterListener();
        }

        private static string GetCleanPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            if (!path.StartsWith("#"))
            {
                return path;
            }

            var dotIndex = path.IndexOf(DataBindSettings.PathSeparator);
            var result = dotIndex < 0 ? null : path.Substring(dotIndex + 1);
            return result;
        }

        /// <summary>
        ///     Converts the specified path to a full, clean path. I.e. replaces the depth value and prepends the master paths.
        /// </summary>
        /// <returns>Full clean path for the specified path.</returns>
        private string GetFullCleanPath(int startDepth, int endDepth)
        {
            var cleanPath = GetCleanPath(this.path);

            var fullPath = cleanPath;
            for (var depth = startDepth; depth < endDepth; ++depth)
            {
                string masterPath;
                if (this.masterPaths.TryGetValue(depth, out masterPath) && !string.IsNullOrEmpty(masterPath))
                {
                    fullPath = masterPath + DataBindSettings.PathSeparator + fullPath;
                }
            }

            return fullPath;
        }

        private static int GetPathDepth(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return 0;
            }

            if (!path.StartsWith("#"))
            {
                return 0;
            }

            var depthString = path.Substring(1);
            var dotIndex = depthString.IndexOf(DataBindSettings.PathSeparator);
            if (dotIndex >= 0)
            {
                depthString = depthString.Substring(0, dotIndex);
            }

            if (depthString == "#")
            {
                return MaxPathDepth;
            }

            int depth;
            if (int.TryParse(depthString, out depth))
            {
                return depth;
            }

            Debug.LogWarning("Failed to get binding context depth for: " + path);
            return 0;
        }

        private void OnContextValueChanged(object newValue)
        {
            if (!this.IsInitialized)
            {
                this.initializer.RemoveInitialization(this);
                this.IsInitialized = true;
            }

            this.OnValueChanged(newValue);
        }

        private void OnValueChanged(object newValue)
        {
            var handler = this.valueChangedCallback;
            if (handler != null)
            {
                handler(newValue);
            }
        }

        /// <summary>
        ///     Registers a callback at the current context.
        /// </summary>
        /// <returns>Current value.</returns>
        private object RegisterListener()
        {
            // Return context itself if no path set.
            if (string.IsNullOrEmpty(this.contextPath))
            {
                return this.context;
            }

            var dataContext = this.context as IDataContext;
            if (dataContext != null)
            {
                try
                {
                    return dataContext.RegisterListener(this.contextPath, this.OnContextValueChanged);
                }
                catch (ArgumentException e)
                {
                    Debug.LogError(e, this.monoBehaviour);
                    return null;
                }
            }

            // If context is not null, but path is set, it is not derived from context class, so 
            // the path can't be resolved. Log an error to inform the user that the context should be derived
            // from the Context class.
            if (this.context != null)
            {
                Debug.LogError(
                    string.Format(
                        "Context of type '{0}' is not derived from '{1}', but path is set to '{2}'. Not able to get data from a non-context type.",
                        this.context.GetType(),
                        typeof(IDataContext),
                        this.contextPath),
                    this.monoBehaviour);
            }

            return null;
        }

        /// <summary>
        ///     Removes the callback from the current context.
        /// </summary>
        private void RemoveListener()
        {
            // Return if no path set.
            if (string.IsNullOrEmpty(this.contextPath))
            {
                return;
            }

            var dataContext = this.context as IDataContext;
            if (dataContext == null)
            {
                return;
            }

            // Remove listener.
            try
            {
                dataContext.RemoveListener(this.contextPath, this.OnContextValueChanged);
            }
            catch (ArgumentException e)
            {
                Debug.LogError(e, this.monoBehaviour);
            }
        }

        /// <summary>
        ///     Updates the master path and context cache.
        /// </summary>
        private void UpdateCache()
        {
            // Clear cache.
            this.contexts.Clear();
            this.masterPaths.Clear();

            var p = this.monoBehaviour.gameObject;

            var depth = 0;

            while (p != null)
            {
                var contextHolder = p.GetComponent<ContextHolder>();
                if (contextHolder != null)
                {
                    if (!this.contexts.ContainsKey(depth))
                    {
                        this.contexts.Add(depth, contextHolder);
                    }

                    // Process path.
                    var pathRest = contextHolder.Path;
                    while (!string.IsNullOrEmpty(pathRest))
                    {
                        var separatorIndex = pathRest.LastIndexOf(DataBindSettings.PathSeparator);
                        string pathSection;
                        if (separatorIndex >= 0)
                        {
                            pathSection = pathRest.Substring(separatorIndex + 1);
                            pathRest = pathRest.Substring(0, separatorIndex);
                        }
                        else
                        {
                            pathSection = pathRest;
                            pathRest = null;
                        }

                        this.masterPaths.Add(depth, pathSection);
                        ++depth;
                    }
                }

                var masterPath = p.GetComponent<MasterPath>();
                if (masterPath != null)
                {
                    // Process path.
                    var pathRest = masterPath.Path;
                    while (!string.IsNullOrEmpty(pathRest))
                    {
                        var separatorIndex = pathRest.LastIndexOf(DataBindSettings.PathSeparator);
                        string pathSection;
                        if (separatorIndex >= 0)
                        {
                            pathSection = pathRest.Substring(separatorIndex + 1);
                            pathRest = pathRest.Substring(0, separatorIndex);
                        }
                        else
                        {
                            pathSection = pathRest;
                            pathRest = null;
                        }

                        this.masterPaths.Add(depth, pathSection);
                        ++depth;
                    }
                }

                p = p.transform.parent == null ? null : p.transform.parent.gameObject;
            }
        }
    }
}