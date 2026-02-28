using UnityEngine;

namespace CodeIsBroken
{
    public static class Time
    {
        public static void SetScale(float scale)
        {
            Tick.Instance.tickTime = 1 / scale;
        }
    }
}
