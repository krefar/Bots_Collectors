using UnityEngine;
using UnityEngine.Animations;

namespace Assets.Scripts.Extensions
{
    public static class Vector3Extensions
    {
        public static bool SamePosition(this Vector3 transform, Vector3 another, Axis axis)
        {
            var result = true;

            if (axis == Axis.None)
            {
                result = false;
            }

            if (axis.HasFlag(Axis.X))
            {
                result = result && Mathf.Approximately(transform.x, another.x);
            }
            
            if (axis.HasFlag(Axis.Y))
            {
                result = result && Mathf.Approximately(transform.y, another.y);
            }
            
            if (axis.HasFlag(Axis.Z))
            {
                result = result && Mathf.Approximately(transform.z, another.z);
            }

            return result;
        }
    }
}