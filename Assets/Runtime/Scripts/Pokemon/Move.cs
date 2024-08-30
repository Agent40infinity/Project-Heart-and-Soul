using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon/Move")]
public class Move : ScriptableObject
{
    public Type type;
    public Category category;
    public int power;
}

[Serializable] public class Moveset
{
    public int level;
    public Move move;
}

public enum Category
{ 
    Physical,
    Special,
    Status
}