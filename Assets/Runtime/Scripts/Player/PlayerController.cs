using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Dictionary<KeyCode, Vector2> keyDict = new Dictionary<KeyCode, Vector2>()
        {
            { KeyCode.W, new Vector2(0, 1) },
            { KeyCode.A, new Vector2(-1, 0) },
            { KeyCode.S, new Vector2(0, -1) },
            { KeyCode.D, new Vector2(1, 1) },
        };
        
        private KeyCode lastKey;

        private Rigidbody rigid;

        public void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        public void Update()
        {
            GetInput();
            Physics();
        }

        public void GetInput()
        {
            if (Input.anyKeyDown)
            {
                var current = Event.current.keyCode;

                if (!keyDict.ContainsKey(current))
                {
                    return;
                }

                lastKey = Event.current.keyCode;
            }
        }

        public void Physics()
        { 
           
        }
    }
}
