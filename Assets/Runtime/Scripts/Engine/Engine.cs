using Runtime.Player;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public static BattleManager Battle;

    public static PlayerData Player;

    public struct PlayerData
    {
        public IPlayerController controller;
        public TrainerInfo trainerInfo;
    }

    public void Awake()
    {
        var playerObj = GameObject.FindWithTag("Player");

        Player.controller = playerObj.GetComponent<IPlayerController>();
        Player.trainerInfo = playerObj.GetComponent<TrainerInfo>();
    }
}