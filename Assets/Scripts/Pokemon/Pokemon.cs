using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "New Pokemon", order = 0)]
public class Pokemon : ScriptableObject
{
    [Header("Pokedex")]
    public int regionalID;
    public int NationalID;

    [Header("Information")]
    public string pokemonName;
    public Type primaryType;
    public Type secondaryType;

    [Header("Details")]
    public ExpType expType;
    public float genderRatio;
    public string classification;
    public string flavorText;
    public int captureRate;
    public int baseHappiness;

    [Header("Possible Abilities")]
    public List<Ability> abilities;

    [Header("Base Stats")]
    public int hP;
    public int attack;
    public int defense;
    public int specialAtk;
    public int specialDef;
    public int speed;

    [Header("Evolution")]
    public List<Evolution> evolution;

    [Header("Moveset & Learnable Moves")]
    public List<Moveset> moveset;
    public List<Move> learnables;
}

[Serializable] public class Evolution
{
    //[Header("Details")]
    public Pokemon evolution;
    public int evoLevel;

    //[Header("Is Item Evolution?")]
    public bool itemRequired;
    public Item item;

    //[Header("Is Conditional?")]
    public bool specialCondition;
    public Condition condition;
}

public enum EggGroup
{ 
    
}

[Serializable] public class ExpType
{
    public Leveling type;

    public enum Leveling { Erratic, Fast, MediumFast, MediumSlow, Slow, Fluctuating }
}