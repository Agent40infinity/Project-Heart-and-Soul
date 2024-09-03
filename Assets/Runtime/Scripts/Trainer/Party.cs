using System;

[Serializable]
public class Party
{
    public Pokemon[] activeParty = new Pokemon[6];

    public void ArrangePokemon(int pokemonIndex, int swapIndex)
    {
        var pokemon = activeParty[pokemonIndex];
        var swap = activeParty[swapIndex];

        activeParty[swapIndex] = pokemon;
        activeParty[pokemonIndex] = swap;
    }
}