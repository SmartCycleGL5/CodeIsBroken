using System;
using UnityEngine;

namespace CodeIsBroken
{
    public struct Color : IEquatable<Color>
    {
        public static Color red => new Color(1, 0, 0);
        public static Color green => new Color(0, 1, 0);
        public static Color blue => new Color(0, 0, 1);
        
        UnityEngine.Color color;
        
        public float r  => color.r;
        public float g  => color.g;
        public float b  => color.b;
        public float a   => color.a;

        public Color(float r, float g, float b, float a = 1)
        {
            color.r = r;
            color.g = g;
            color.b = b;
            color.a = a;
        }
        
        public bool Equals(Color other)
        {
            return color == other.color;
        }
    }
}
