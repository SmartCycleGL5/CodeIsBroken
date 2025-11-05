using System;
using SharpCube.Highlighting;

namespace SharpCube
{

    public class Keyword
    {
        public string key { get; set; }
        public virtual string color => SyntaxHighlighting.ColorPallate.defaultColor;

        public Keyword(string key)
        {
            this.key = key;
        }
    }

    public class Initializer : Keyword
    {
        public Action<IContainer, Line, int, Properties> Intialize { get; private set; }
        public override string color => SyntaxHighlighting.ColorPallate.initializerColor;

        public Initializer(string key, Action<IContainer, Line, int, Properties> Intialize) : base(key)
        {
            this.Intialize = Intialize;
        }
    }
    public class Modifier : Keyword
    {
        public Privilege privilege { get; set; }
        public override string color => SyntaxHighlighting.ColorPallate.modifierColor;

        public Modifier(string key, Privilege privilege): base(key)
        {
            this.privilege = privilege;
        }
    }

    public class Reference : Keyword
    {
        public Variable variable { get; private set; }
        public override string color => SyntaxHighlighting.ColorPallate.variableColor;
        
        public Reference(string key, Variable variable) : base(key)
        {
            this.variable = variable;
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