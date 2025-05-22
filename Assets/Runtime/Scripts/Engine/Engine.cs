using Runtime.Player;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public static BattleManager Battle;

    public static IPlayer Player;

    public void Awake()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<IPlayer>();
    }
}