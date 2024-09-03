using System;
using UnityEngine;

[Serializable]
public struct Passage
{
    [SerializeField] public SceneHandle handle;
    [SerializeField] public int connection;

    public static implicit operator SceneHandle(Passage passage)
    {
        return passage.handle.Connections[passage.connection].targetPassage.handle;
    }

    public string Name => handle.Connections[connection].identity;

    public string TargetName => handle.Connections[connection].targetPassage.Name;
}
