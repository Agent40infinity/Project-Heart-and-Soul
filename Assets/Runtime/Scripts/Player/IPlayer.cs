using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Player
{
    public interface IPlayer
    {
        Vector3 FutureDist();
        Vector3? PastPos();
        bool Running();
        float Speed();
        bool IsMoving();
        void SubscribeOnStep(UnityAction call);
        void UnsubscribeOnStep(UnityAction call);
    }
}