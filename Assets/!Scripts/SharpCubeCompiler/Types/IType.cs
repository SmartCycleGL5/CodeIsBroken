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
        
        public static void CreateMethod(IContainer encapsulation, Line line, int initializer, Properties properties)
        {
            PlayerConsole.Log("Wow nice method bro, would be a  shame if i didnt compile it");
        }

        public static IType NewType(Types type)
        {
            switch (type)
            {
                case Types.Void:
                    throw new NotImplementedException();
                    break;
                case Types.Bool:
                    return new Bool();
                    break;
                case Types.Int:
                    return new Int();
                    break;
                case Types.String:
                    return new String();
                    break;
                case Types.Float:
                    return new Float();
                    break;
            }
            
            throw new NotImplementedException();
        }
    }
}
