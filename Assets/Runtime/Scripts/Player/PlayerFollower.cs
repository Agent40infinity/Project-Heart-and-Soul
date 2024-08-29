using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Overworld;

namespace Runtime.Player
{
    public class PlayerFollower : MonoBehaviour
    {
        IPlayer player;

        public Vector3 toMove = Vector3.zero;

        public void Awake()
        {
            player = GameObject.Find("Player").GetComponent<IPlayer>();
        }

        void Update()
        {
            if (transform.position != player.PastPos())
            {
                toMove = player.PastPos();
                toMove.x = Mathf.Round(toMove.x);
                toMove.z = Mathf.Round(toMove.z);
            }

            transform.position = OverworldPhysics.Update(transform, ref toMove, player.Speed(), player.Running());
        }
    }
}