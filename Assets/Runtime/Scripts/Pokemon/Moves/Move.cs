using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Pokemon/Move")]
public class Move : ScriptableObject
{
    
}

[Serializable] public class Moveset
{
    public int level;
    public Move move;
}