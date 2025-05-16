using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EncounterTable")]
public class EncounterTable : ScriptableObject
{
    public List<WildPokemon> pokemon = new List<WildPokemon>();
}
