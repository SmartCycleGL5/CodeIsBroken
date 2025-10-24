using System;
using UnityEngine;

namespace SharpCube
{
    public interface IKeyword
    {
        public string color {  get; set; }
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
        public bool @abstract;

        public Properties(Privilege privilege, bool @static, bool @abstract)
        {
            this.privilege = privilege;
            this.@static = @static;
            this.@abstract = @abstract;
        }
    }
}