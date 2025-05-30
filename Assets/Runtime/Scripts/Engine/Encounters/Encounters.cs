using UnityEngine;

public class Encounters : MonoBehaviour
{
    public EncounterTable encounterTable;

    private int CheckRate => Random.Range(0, 2880);

    private void Start()
    {
        Engine.Player.controller.SubscribeOnStep(StepTaken);
    }

    private void OnDestroy()
    {
        Engine.Player.controller.UnsubscribeOnStep(StepTaken);
    }

    public void StepTaken()
    {
        if (CheckRate > 320)
        {
            return;
        }

        RollEncounter();
    }

    public void RollEncounter()
    {
        var chance = Random.Range(1, 101);
        var total = 0;

        foreach (var encounter in encounterTable.pokemon)
        {
            total += encounter.rate;

            if (chance <= total)
            {
                Engine.Battle.SetupBattle(GenerateWildTrainer(GenerateWildPokemon(encounter)));
                break;
            }
        }
    }

    public Pokemon GenerateWildPokemon(WildPokemon encounter)
    {
        var level = Random.Range(encounter.level.minLvl, encounter.level.maxLvl + 1);

        return new Pokemon().GenerateWild(encounter.pokemon, level);
    }

    public ITrainer GenerateWildTrainer(Pokemon pokemon)
    {
        return new trainer(Intelligence.Wild, pokemon);
    }
}