using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public interface IPlayer
    {
        Vector3 CurrentKey();
        Vector3 FuturePos();
        bool Running();
    }
}