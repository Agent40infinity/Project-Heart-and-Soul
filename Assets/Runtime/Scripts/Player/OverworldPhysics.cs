using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Player
{
    public static class OverworldPhysics
    {
        public static Vector3 Update(List<KeyCode> keys, Transform entity, ref Vector3 toMove, float entitySpeed, bool running)
        {
            if (keys.Count > 0)
            {
                toMove = entity.position;
                toMove += PlayerController.keyDict[keys.Last()];
                toMove.x = Mathf.Round(toMove.x);
                toMove.z = Mathf.Round(toMove.z);
            }

            return Vector3.MoveTowards(entity.position, toMove, (running ? entitySpeed * 2 : entitySpeed) * Time.deltaTime);
        }

        public static Vector3 Update(IPlayer player, Transform entity, ref Vector3 toMove, float entitySpeed, bool running)
        {
            toMove = entity.position;
            toMove += player.CurrentKey() * -1;
            toMove.x = Mathf.Round(toMove.x);
            toMove.z = Mathf.Round(toMove.z);

            return Vector3.MoveTowards(entity.position, toMove, (running ? entitySpeed * 2 : entitySpeed) * Time.deltaTime);
        }
    }
}