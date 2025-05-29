using System;

[Serializable]
public class Party
{
    public Pokemon[] pokemon = new Pokemon[6];

    public void ArrangePokemon(int pokemonIndex, int swapIndex)
    {
        var pokemon = this.pokemon[pokemonIndex];
        var swap = this.pokemon[swapIndex];

        this.pokemon[swapIndex] = pokemon;
        this.pokemon[pokemonIndex] = swap;
    }
}