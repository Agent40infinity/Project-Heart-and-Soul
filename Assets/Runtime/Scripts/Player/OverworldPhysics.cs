using UnityEngine;

namespace Runtime.Overworld
{
    public static class OverworldPhysics
    {
        public static bool WithinTile(Vector3 toMove, Transform entity)
        {
            return (toMove - entity.position).sqrMagnitude < Mathf.Epsilon;
        }

        public static void RoundTowards(Vector3 toMove)
        {
            Mathf.Round(toMove.x);
            Mathf.Round(toMove.z);
        }

        public static Vector3 Update(Transform entity, ref Vector3 toMove, float entitySpeed, bool running)
        {
            return Vector3.MoveTowards(entity.position, toMove, (running ? entitySpeed * 2 : entitySpeed) * Time.deltaTime);
        }
    }
}