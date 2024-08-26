using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Dictionary<KeyCode, Vector3> keyDict = new Dictionary<KeyCode, Vector3>()
        {
            { KeyCode.W, new Vector3(0, 0, 1) },
            { KeyCode.A, new Vector3(-1, 0, 0) },
            { KeyCode.S, new Vector3(0, 0, -1) },
            { KeyCode.D, new Vector3(1, 0, 0) },
        };

        private KeyCode lastKey = KeyCode.None;

        public float playerSpeed = 2f;
        private Vector3 toMove = new Vector3();

        public bool running = false;

        public void Update()
        {
            Physics();
        }

        public void OnGUI()
        {
            GetInput();
        }
        public void GetInput()
        {
            if (Input.anyKey)
            {
                var current = Event.current.keyCode;

                if (!keyDict.ContainsKey(current))
                {
                    return;
                }

                running = Event.current.shift;

                lastKey = Event.current.keyCode;
                return;
            }

            lastKey = KeyCode.None;
        }

        public void Physics()
        {
            if (lastKey != KeyCode.None)
            {
                toMove = transform.position;
                toMove += keyDict[lastKey];
            }

            transform.position = Vector3.MoveTowards(transform.position, toMove, (running ? playerSpeed * 2 : playerSpeed) * Time.deltaTime);
        }
    }
}
