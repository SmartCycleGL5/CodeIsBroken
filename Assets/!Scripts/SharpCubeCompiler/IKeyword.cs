using System;
using SharpCube.Highlighting;

namespace SharpCube
{

    public class Keyword
    {
        public string name { get; set; }
        public virtual string color => SyntaxHighlighting.ColorPallate.defaultColor;

        public Keyword(string name)
        {
            this.name = name;
        }
    }

    public class Initializer : Keyword
    {
        public Action<IContainer, Line, int, Properties> Intialize { get; private set; }
        public override string color => SyntaxHighlighting.ColorPallate.initializerColor;

        public Initializer(string name, Action<IContainer, Line, int, Properties> Intialize) : base(name)
        {
            this.Intialize = Intialize;
        }
    }
    public class Modifier : Keyword
    {
        public Privilege privilege { get; set; }
        public override string color => SyntaxHighlighting.ColorPallate.modifierColor;

        public Modifier(string name, Privilege privilege): base(name)
        {
            this.privilege = privilege;
        }
    }

    public class Reference : Keyword
    {
        public override string color => SyntaxHighlighting.ColorPallate.variableColor;
        
        public Reference(string name) : base(name)
        {
        }
    }
    public class Reference<T> : Reference where T : IReference
    {
        public T reference { get; private set; }
        
        public Reference(string name, T reference) : base(name)
        {
            this.reference = reference;
        }
    }

    public class Properties
    {
        public Privilege privilege;
        public bool @static;

        public Properties(Privilege privilege = Privilege.None, bool @static = false)
        {
            this.privilege = privilege;
            this.@static = @static;
        }
    }
}