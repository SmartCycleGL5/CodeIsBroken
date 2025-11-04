using SharpCube.Type;
using System;
using UnityEngine;

namespace SharpCube
{
    public class Method : IReference
    {
        public string name { get; set; }
        public IContainer container { get; set; }

        public static void Create(IContainer container, Line line, int initializer, Properties properties)
        {
            

            PlayerConsole.LogWarning("Methods not implemented");
            //string name = line.sections[initializer + 1];
            //string type = line.sections[initializer];

            //try
            //{
            //    Types T = Enum.Parse<Types>(type, true);
            //    new Variable(name, properties, encapsulation, IType.NewType(T));
            //}
            //catch
            //{
            //    new Variable(name, properties, encapsulation, null);
            //}
        }
    }

}