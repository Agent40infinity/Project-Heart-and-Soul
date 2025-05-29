using Runtime.Player;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public static BattleManager Battle;

    public static IPlayerController Player;
    public static TrainerInfo PlayerTrainerInfo;

    public void Awake()
    {
        var playerObj = GameObject.FindWithTag("Player");

        Player = playerObj.GetComponent<IPlayerController>();
        PlayerTrainerInfo = playerObj.GetComponent<TrainerInfo>();
    }
}