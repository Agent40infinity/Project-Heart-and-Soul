// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataContextNodeConnectorInitializer.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///     Singleton which takes care about the initialization of <see cref="DataContextNodeConnector" /> objects,
    ///     so they are initialized correctly.
    ///     The connectors have to be initialized in the LateUpdate of the frame they are activated, so other connectors
    ///     have the chance to get their correct initial value, too.
    /// </summary>
    public class DataContextNodeConnectorInitializer
    {
        private readonly List<Initialization> initializations = new List<Initialization>();

        /// <summary>
        ///     Has to be called at the end of each frame to perform all scheduled initializations.
        /// </summary>
        public void ExecuteInitializations()
        {
            while (this.initializations.Count > 0)
            {
                var scheduledInitializations = new List<Initialization>(this.initializations);
                this.initializations.Clear();
                foreach (var initialization in scheduledInitializations)
                {
                    initialization.Connector.Init(initialization.InitialValue);
                }
            }
        }

        /// <summary>
        ///     Registers an initialization for a connector with the passed initial value.
        /// </summary>
        /// <param name="connector">Connector to initialize.</param>
        /// <param name="initialValue">Initial value of the connector.</param>
        /// <exception cref="InvalidOperationException">Thrown if an initialization was already registered for the connector.</exception>
        public void RegisterInitialization(DataContextNodeConnector connector, object initialValue)
        {
            // Check if there is already an initialization registered for the connector.
            if (this.initializations.Any(initialization => initialization.Connector == connector))
            {
                throw new InvalidOperationException("There is already an initialization registered for the connector");
            }

            this.initializations.Add(new Initialization {Connector = connector, InitialValue = initialValue});
        }

        /// <summary>
        ///     Removes a registered initialization for the specified connector.
        /// </summary>
        /// <param name="connector">Connector to remove scheduled initialization for.</param>
        /// <returns>True if there was an initialization which was removed; otherwise, false.</returns>
        public bool RemoveInitialization(DataContextNodeConnector connector)
        {
            return this.initializations.RemoveAll(initialization => initialization.Connector == connector) != 0;
        }

        private class Initialization
        {
            public DataContextNodeConnector Connector;

            public object InitialValue;
        }
    }
}