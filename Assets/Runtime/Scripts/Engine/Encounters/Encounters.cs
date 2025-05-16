using UnityEngine;

public class Encounters : MonoBehaviour
{
    public EncounterTable encounterTable;

    private int CheckRate => Random.Range(1, 257);

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
        foreach (var encounter in encounterTable.pokemon)
        {
            if (CheckRate <= encounter.rate)
            {
                Debug.Log("Encountered: " + encounter.pokemon.name + "!");
                break;
            }    
        }
    }

    /*public void RollEncounter()
    {
        var chance = CheckRate;
        var totalRate = 0;

        foreach (var encounter in encounterTable)
        {
            totalRate += encounter.rate;

            if (chance <= totalRate)
            {
                Debug.Log("Encountered: " + encounter.pokemon.name + "!");
                return;
            }
        }
    }*/
}
