using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BattleManager : MonoBehaviour
{
    private void Awake()
    {
        Engine.Battle = this;
    }

    public void SetupBattle(ITrainer opponent)
    { 
        
    }
}