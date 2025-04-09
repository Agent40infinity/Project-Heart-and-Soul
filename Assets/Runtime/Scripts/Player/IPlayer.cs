using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Player
{
    public interface IPlayer
    {
        Vector3 FutureDist();
        Vector3? PastPos();
        bool Running();
        float Speed();
        bool IsMoving();
    }
}