using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable] public class Pokemon
{
    [Header("Identification")]
    public string nickname;
    public string oT;
    public PokemonBase origin;

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
    public int level;
    public int exp;

    [Header("IVs & EVs")]
    public int[] iVs;
    public int[] eVs;

    [Header("Learnt Moves")]
    public Move[] moves;

    public int HP()
        => 0;
    public int Atk() 
        => 0;
    public int Def() 
        => 0;
    public int SpAtk() 
        => 0;
    public int SpDef() 
        => 0;
    public int Spd() 
        => 0;
}

public enum Gender
{
    Male,
    Female,
    Genderless,
}