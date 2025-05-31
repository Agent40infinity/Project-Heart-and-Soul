using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BattleManager : MonoBehaviour
{
    List<ITrainer> trainers = new List<ITrainer>();

    private void Awake()
    {
        Engine.Battle = this;

        trainers.Add(Engine.Player.trainerInfo.trainer);
    }

    public void SetupBattle(List<ITrainer> opponents)
    {
        trainers.AddRange(opponents);
    }

    public void SetupBattle(ITrainer opponent)
    {
        trainers.Add(opponent);
    }

    public void EndBattle()
    {
        trainers.RemoveRange(1, trainers.Count - 1);
    }


}