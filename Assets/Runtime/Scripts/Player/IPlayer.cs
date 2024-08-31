using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Player
{
    public interface IPlayer
    {
        Vector3 CurrentKey();
        Vector3 PastPos();
        bool Running();
        float Speed();
        bool IsMoving();
    }
}