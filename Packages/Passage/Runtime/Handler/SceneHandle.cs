using System.Collections.Generic;
using UnityEngine;
using DevLocker.Utils;

[System.Serializable]
[CreateAssetMenu(menuName = "Scene Handle")]
public class SceneHandle : ScriptableObject
{
    public PairVar<SceneReference, List<SceneReference>> scene;
    public WorldArea area;

    [Space(20)]
    [SerializeField] private List<SceneData> connections = new List<SceneData>();
    public List<SceneData> Connections => connections;
}