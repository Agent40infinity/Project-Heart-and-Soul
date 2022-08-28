using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable] public class CaughtPokemon
{
    [Header("Identification")]
    public int uniqueID;
    public string nickname;
    public string oT;

    [Header("Description")]
    public Pokeball caughtWith;
    public string metDate;
    public string metDescription;
    public string personality;

    [Header("Information")]
    public Gender gender;
    public Nature nature;
    public Ability ability;
    public bool shiny;

    [Header("Details")]
    public int happiness;
    public Item heldItem;

    [Header("Experience")]
    public int exp;

    [Header("IVs & EVs")]
    public int[] iVs;
    public int[] eVs;

    [Header("Learnt Moves")]
    public Move[] moves;
}

public enum Gender
{
    Male,
    Female,
    Genderless,
}