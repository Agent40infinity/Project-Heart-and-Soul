public static class Damage
{
    public static float Calculate(Pokemon attacker, Pokemon target, Move moveUsed)
    {
        float dmg = (((((2 * attacker.level / 5) + 2)
            * moveUsed.power
            * (moveUsed.category == Category.Physical ? attacker.Atk() / target.Def() : attacker.SpAtk() / target.SpDef()))
            / 50)
            + 2) // Burn * Screen * Targets * Weather * FF + 2;
            * RollCrit()
            * Item()
            * Ability()
            * (moveUsed.name.Contains("Me First") ? 1.5f : 1) // First
            * RollRandom()
            * STAB(attacker.origin, moveUsed.type)
            * TypeMultiplier(attacker.origin.primaryType, moveUsed.type)
            * TypeMultiplier(attacker.origin.secondaryType, moveUsed.type)
            * 1f; // Berry Weakening Check, 0.5 if defender is holding berry

        return dmg;
    }

    private static float TypeMultiplier(Type pokeType, Type moveType)
    {
        if (moveType.effective.Contains(pokeType))
        {
            return 2;
        }

        if (moveType.weakness.Contains(pokeType))
        {
            return 0.5f;
        }

        if (moveType.immunity.Contains(pokeType))
        {
            return 0;
        }

        return 1;
    }

    private static int RollCrit()
        => UnityEngine.Random.value <= 0.0625 ? 2 : 1;

    // TODO: 
    private static float Item() 
        // Expert Belt Check, 1.2 if held item
        => 1;

    private static float Ability()
        // target ability: Solid Rock or Filter, 0.75 if super effective. If Attacker Ability = Mold Breaker, 1
        // Tinted Lens, 2 if not very effective.
        => 1;

    private static float RollRandom()
        => UnityEngine.Random.Range(85, 101) / 100;

    //Same-Type Attack Bonus
    private static int STAB(PokemonBase pokeBase, Type moveType)
        => pokeBase.primaryType == moveType || pokeBase.secondaryType == moveType ? 2 : 1;
}