using System.Linq;
using UnityEngine;

[System.Serializable] public class Pokemon
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
    public int hpIV;
    public int atkIV;
    public int defIV;
    public int spAtkIV;
    public int spDefIV;
    public int spdIV;

    public int hpEV;
    public int atkEV;
    public int defEV;
    public int spAtkEV;
    public int spDefEV;
    public int spdEV;

    public int remainingEVs = 510;

    [Header("Learnt Moves")]
    public Move[] moves = new Move[4];

    public int Hp() => Mathf.FloorToInt(0.01f * (2 * origin.hp + hpIV + Mathf.FloorToInt(0.25f * hpEV)) * level) + level + 10;
    public int Atk() => CalculateOtherStat(origin.attack, atkIV, atkEV);
    public int Def() => CalculateOtherStat(origin.defense, defIV, defEV);
    public int SpAtk() => CalculateOtherStat(origin.specialAtk, spAtkIV, spAtkEV);
    public int SpDef() => CalculateOtherStat(origin.specialDef, spDefIV, spDefEV);
    public int Spd() => CalculateOtherStat(origin.speed, spdIV, spdEV);

    public int CalculateOtherStat(int otherBase, int otherIV, int otherEV)
        => (Mathf.FloorToInt(0.01f * (2 * otherBase + otherIV + Mathf.FloorToInt(0.25f * otherEV)) * level) + 5) * 1; // Replace 1 with Nature.

    public Pokemon GenerateWild(PokemonBase pokeBase, int lvl)
    {
        origin = pokeBase;
        level = lvl;

        ability = origin.abilities[Random.Range(0, origin.abilities.Count)];
        gender = (Gender)Random.Range(0, 2);

        hpIV = Random.Range(0, 32);
        atkIV = Random.Range(0, 32);
        defIV = Random.Range(0, 32);
        spAtkIV = Random.Range(0, 32);
        spDefIV = Random.Range(0, 32);
        spdIV = Random.Range(0, 32);

        var avaliableMoves = origin.moveset.Where(ms => ms.level <= level).ToList();
        moves = avaliableMoves
            .GetRange(Mathf.Clamp(avaliableMoves.Count - 5, 0, 100), Mathf.Clamp(moves.Length, 1, avaliableMoves.Count))
            .Select(m => m.move)
            .ToArray();

        return this;
    }
}

public enum Gender
{
    Male,
    Female,
    Genderless,
}