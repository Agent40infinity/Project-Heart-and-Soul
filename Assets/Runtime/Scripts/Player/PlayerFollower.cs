using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerFollower : MonoBehaviour
    {
        IPlayer player;

        public void Awake()
        {
            player = GameObject.Find("Player").GetComponent<IPlayer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (player.CurrentKey() == Vector3.zero)
            {
                return;
            }

            
        }
    }
}