using ScriptEditor.Console;
using SharpCube;
using System;

namespace SharpCube.Type
{
    public enum Types
    {
        Void,
        Bool,
        Int,
        Float,
        String,
    }
    
    public interface IType<T> : IType
    {
        public T Value { get; set; }
    }
    public interface IType
    {
        public static void Create(IContainer encapsulation, Line line, Properties properties)
        {

            string name = line.sections[line.sections.Length - 2];

            if (line.sections[line.sections.Length - 1] == ")")
            {
                PlayerConsole.Log("Wow nice method bro, would be a  shame if i didnt compile it");
            }

            if (line.sections[line.sections.Length - 1] == ";")
            {
                new Variable(name, properties, encapsulation, new Int());
            }

        }

        public static IType NewType(Types type)
        {
            switch (type)
            {
                case Types.Void:
                    throw new NotImplementedException();
                case Types.Bool:
                    return new Bool();
                case Types.Int:
                    return new Int();
                case Types.String:
                    return new String();
                case Types.Float:
                    return new Float();
            }
            
            throw new NotImplementedException();
        }
    }
}
