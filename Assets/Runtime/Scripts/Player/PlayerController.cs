using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour, IPlayer
    {
        public static Dictionary<KeyCode, Vector3> keyDict = new Dictionary<KeyCode, Vector3>()
        {
            { KeyCode.W, new Vector3(0, 0, 1) },
            { KeyCode.A, new Vector3(-1, 0, 0) },
            { KeyCode.S, new Vector3(0, 0, -1) },
            { KeyCode.D, new Vector3(1, 0, 0) },
        };

        public List<KeyCode> lastKeys = new List<KeyCode>();

        public float playerSpeed = 2f;
        private Vector3 toMove = new Vector3();

        private bool running = false;

        public Vector3 CurrentKey()
            => lastKeys.Count() > 0 ? keyDict[lastKeys.Last()] : Vector3.zero;

        public KeyCode LastKey;

        public Vector3 FuturePos()
            => toMove - transform.position;

        public bool Running()
            => running;

        public void Update()
        {
            GetInput();
            Physics();
        }

        public void GetInput()
        {
            foreach (var key in keyDict.Keys)
            {
                if (Input.GetKeyDown(key))
                {
                    lastKeys.Add(key);
                    LastKey = key;
                }

                if (Input.GetKeyUp(key))
                {
                    lastKeys.Remove(key);
                }
            }

            running = Input.GetKey(KeyCode.LeftShift);
        }

        public void Physics()
        {
            transform.position = OverworldPhysics.Update(lastKeys, transform, ref toMove, playerSpeed, running);
        }
    }
}