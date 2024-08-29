using UnityEngine;

namespace Runtime.Overworld
{
    public static class OverworldPhysics
    {
        public static Vector3 Update(Transform entity, ref Vector3 toMove, float entitySpeed, bool running)
        {
            return Vector3.MoveTowards(entity.position, toMove, (running ? entitySpeed * 2 : entitySpeed) * Time.deltaTime);
        }
    }
}