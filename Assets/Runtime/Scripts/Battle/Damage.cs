public class Damage
{
    public float Calculate(Pokemon attacker, Pokemon target, Move moveUsed)
    {
        float dmg = ((2 * attacker.level / 5) + 2) * moveUsed.power;
        dmg *= moveUsed.category == Category.Physical ? attacker.Atk() / target.Def() : attacker.SpAtk() / target.SpDef();
        dmg /= 50;
        dmg *= 2; //Burn * Screen * Targets * Weather * FF + 2;
        dmg *= RollCrit()
            * Item()
            * RollRandom()
            // * First
            * STAB(attacker.origin, moveUsed.type)
            * TypeMultiplier(attacker.origin.primaryType, moveUsed.type)
            * TypeMultiplier(attacker.origin.secondaryType, moveUsed.type);
            // * SRF
            // * EB
            // * TL
            // * Berry

        return dmg;
    }

    private float TypeMultiplier(Type pokeType, Type moveType)
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

    private int RollCrit()
        => UnityEngine.Random.value <= 0.0625 ? 2 : 1;

    // TODO: 
    private float Item()
        => 1;

    private float RollRandom()
        => UnityEngine.Random.Range(0.85f, 1);

    //Same-Type Attack Bonus
    private int STAB(PokemonBase pokeBase, Type moveType)
        => pokeBase.primaryType == moveType || pokeBase.secondaryType == moveType ? 2 : 1;
}