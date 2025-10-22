using System;
using UnityEngine;
namespace SharpCube.Object
{
    [Serializable]
    public class Class
    {
        public string name;

        public Memory<object> variables;

        public Class(string name, Properties properties)
        {
            Debug.Log("[Class] new class");

            this.name = name;

            Compiler.toCompile.classes.Add(name, this, properties.privilege);
        }

        public static void Create(int line)
        {
            Properties properties = new Properties();

            for (int i = line; i >= 0; i--)
            {
                if (Compiler.convertedCode[i] == "private") return;
            }

            new Class(Compiler.convertedCode[line + 1], properties);
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
}
