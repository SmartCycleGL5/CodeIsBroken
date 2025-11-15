using UnityEngine;

namespace CodeIsBroken
{
    public static class Random
    {
        public static int RangeOf(int min, int max)
        {
            return UnityEngine.Random.Range(min, max + 1);
        }
    }
}
