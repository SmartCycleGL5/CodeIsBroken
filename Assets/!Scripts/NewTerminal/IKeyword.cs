using System;
using UnityEngine;

namespace SharpCube
{
    public enum Keyword
    {
        Class,
        Struct
    }

    public interface IKeyword
    {
        public Keyword key { get; set; }
        public Color color {  get; set; }
        public Action<int> create { get; set; }
    }

    public struct Encapsulation : IKeyword
    {
        public Keyword key { get; set; }
        public Color color { get; set; }
        public Action<int> create { get; set; }

        public Encapsulation(Keyword key, Color color, Action<int> create)
        {
            this.key = key;
            this.color = color;
            this.create = create;
        }
    }
}