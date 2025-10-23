using System;
using UnityEngine;

namespace SharpCube
{
    public interface IKeyword
    {
        public string key { get; set; }
        public Color color {  get; set; }
    }

    public struct Encapsulation : IKeyword
    {
        public string key { get; set; }
        public Color color { get; set; }
        public Action<int> create { get; set; }

        public Encapsulation(string key, Color color, Action<int> create)
        {
            this.key = key;
            this.color = color;
            this.create = create;
        }
    }
    public struct Accessibility : IKeyword
    {
        public string key { get; set; }
        public Color color { get; set; }
        public Privilege privilege { get; set; }

        public Accessibility(string key, Color color, Privilege privilege)
        {
            this.key = key;
            this.color = color;
            this.privilege = privilege;
        }
    }
    public struct Type<T> : IKeyword
    {
        public string key { get; set; }
        public Color color { get; set; }

        T GetType()
        {
            return default; 
        }
    }
}