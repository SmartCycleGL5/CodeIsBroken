using SharpCube;
using System;

namespace SharpCube.Type
{
    public interface IType
    {
        
        public static void Create(Encapsulation encapsulation, Line line, Properties properties)
        {
            
            string name = line.sections[line.sections.Length - 2];

            if (line.sections[line.sections.Length - 1] == ")")
            {
                PlayerConsole.Log("Wow nice method bro, would be a  shame if i didnt compile it");
            }

            if (line.sections[line.sections.Length - 1] == ";")
            {
                new Variable(name, properties, encapsulation);
            }
            
        }
    }
}
