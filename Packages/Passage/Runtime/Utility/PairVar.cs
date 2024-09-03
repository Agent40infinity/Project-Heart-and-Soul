using System;

[Serializable]
public struct PairVar<T, L>
{
    [UnityEngine.SerializeField] private bool enabled;
    [UnityEngine.SerializeField] private T value;
    [UnityEngine.SerializeField] private L alt;

    public PairVar(T initial, L altInitial, bool isDefault)
    {
        enabled = isDefault;
        value = initial;
        alt = altInitial;
    }

    public bool Enabled => enabled;
    public T Initial => value;
    public L Alt => alt;

    public dynamic Value => enabled ? value : alt;
}