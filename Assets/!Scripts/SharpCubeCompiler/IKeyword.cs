using System;
using System.Collections.Generic;
using UnityEngine;

namespace SharpCube
{

    public class Keyword
    {
        public string key { get; set; }
        public string color { get; set; }

        public Keyword(string key, string color)
        {
            this.key = key;
            this.color = color;
        }
    }

    public class Initializer : Keyword
    {
        public Action<Encapsulation, Line, int, Properties> create { get; set; }

        public Initializer(string key, string color, Action<Encapsulation, Line, int, Properties> create) : base(key, color)
        {
            this.create = create;
        }
    }
    public class Modifier : Keyword
    {
        public Privilege privilege { get; set; }

        public Modifier(string key, string color, Privilege privilege): base(key, color)
        {
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