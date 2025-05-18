using UnityEngine;

public class Encounters : MonoBehaviour
{
    public EncounterTable encounterTable;

    private int CheckRate => Random.Range(0, 2880);

    private void Start()
    {
        Engine.Player.SubscribeOnStep(StepTaken);
    }

    private void OnDestroy()
    {
        Engine.Player.UnsubscribeOnStep(StepTaken);
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
                Debug.Log("Encountered: " + encounter.pokemon.name + "!");
                return;
            }
        }
    }
}