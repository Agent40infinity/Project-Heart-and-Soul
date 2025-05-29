using UnityEngine;

[System.Serializable]
public class trainer : ITrainer
{
    [SerializeField] private Bag bag = new Bag();
    [SerializeField] private Party party = new Party();

    public Intelligence intelligence;

    public Bag Bag() => bag;

    public Party Party() => party;

    public trainer(Intelligence aiType, Pokemon pokemon)
    {
        intelligence = aiType;
        party.pokemon[0] = pokemon;
    }
}

public enum Intelligence { Wild = 0, Standard = 1, Advanced = 2, Player = 3 }