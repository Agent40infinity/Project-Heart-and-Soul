// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBindRunner.cs" company="Slash Games">
//   Copyright (c) Slash Games. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Slash.Unity.DataBind.Core.Presentation
{
    using UnityEngine;

    /// <summary>
    ///     Singleton script which uses Unity callbacks to execute global logic (e.g. for initialization).
    /// </summary>
    public class DataBindRunner : MonoBehaviour
    {
        private static DataBindRunner instance;

        private DataContextNodeConnectorInitializer dataContextNodeConnectorInitializer;

        public DataContextNodeConnectorInitializer DataContextNodeConnectorInitializer
        {
            get
            {
                return this.dataContextNodeConnectorInitializer ?? (this.dataContextNodeConnectorInitializer =
                           new DataContextNodeConnectorInitializer());
            }
        }

        public static DataBindRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<DataBindRunner>();

                    if (instance == null)
                    {
                        // Create a data bind runner which takes care about the initialization of the connectors.
                        var dataBindRunnerGameObject = new GameObject("Data Bind Runner");
                        if (Application.isPlaying)
                        {
                            DontDestroyOnLoad(dataBindRunnerGameObject);
                        }

                        instance = dataBindRunnerGameObject.AddComponent<DataBindRunner>();
                    }
                }

                return instance;
            }
        }

        public void LateUpdate()
        {
            this.DataContextNodeConnectorInitializer.ExecuteInitializations();
        }
    }
}