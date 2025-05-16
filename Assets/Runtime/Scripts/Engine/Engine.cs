using Runtime.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public static BattleManager Battle = new BattleManager();

    public static IPlayer Player;

    public void Awake()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<IPlayer>();
    }
}