using System;
using UnityEngine;

namespace SharpCube
{
    public interface IKeyword
    {
        public string color {  get; set; }
    }

    public struct Keyword : IKeyword
    {
        public string color { get; set; }

        public Keyword(string color)
        {
            this.color = color;
        }
    }

    public struct Initializer : IKeyword
    {
        public string color { get; set; }
        public Action<int, Properties> create { get; set; }

        public Initializer(string color, Action<int, Properties> create)
        {
            this.color = color;
            this.create = create;
        }
    }
    public struct Modifier : IKeyword
    {
        public string color { get; set; }
        public Privilege privilege { get; set; }

        public Modifier(string color, Privilege privilege)
        {
            this.color = color;
            this.privilege = privilege;
        }
    }

    public struct Properties
    {
        public Privilege privilege;
        public bool @static;

        public Properties(Privilege privilege, bool @static)
        {
            this.privilege = privilege;
            this.@static = @static;
        }
    }
}