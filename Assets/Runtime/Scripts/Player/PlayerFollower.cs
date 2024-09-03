using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Overworld;

namespace Runtime.Player
{
    public class PlayerFollower : MonoBehaviour
    {
        IPlayer player;

        public float facing = 2;
        public Vector3 toMove = Vector3.zero;

        private Animator anim;

        public void Awake()
        {
            player = GameObject.Find("Player").GetComponent<IPlayer>();
            anim = GetComponentInChildren<Animator>();
        }

        void Update()
        {
            if (OverworldPhysics.WithinTile(toMove, transform))
            {
                if (transform.position != player.PastPos())
                {
                    toMove = player.PastPos();
                    OverworldPhysics.RoundTowards(toMove);
                }
            }

            transform.position = OverworldPhysics.Update(transform, ref toMove, player.Speed(), player.Running());

            Facing();
        }

        private void Facing()
        {
            switch (true)
            {
                case var _ when toMove.x > transform.position.x:
                    facing = 2;
                    break;
                case var _ when toMove.x < transform.position.x:
                    facing = 4;
                    break;
                case var _ when toMove.z > transform.position.z:
                    facing = 1;
                    break;
                case var _ when toMove.z < transform.position.z:
                    facing = 3;
                    break;
            }

            anim.SetFloat("facing", facing);
        }
    }
}