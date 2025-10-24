using System;
using UnityEngine;
namespace SharpCube.Object
{
    [Serializable]
    public class Class : IObject, IContainer
    {
        public string name { get; set; }

        public static Memory<Class> initializedClasses { get; private set; } = new();

        [field: SerializeField] public Memory<Variable> variables { get; set; }

        public Class(string name, Properties properties)
        {
            Debug.Log($"[Class] new class {name}");

            this.name = name;

            initializedClasses.Add(name, this, properties.privilege);
            Compiler.toCompile.classes.Add(name, this, properties.privilege);
        }

        public static void Create(int line, Properties properties)
        {
            string name = Compiler.convertedCode[line + 1];

            if (initializedClasses.Contains(name))
            {
                PlayerConsole.LogError($"The class {name} is already in use");
                throw new Exception("Player error");
            }

            new Class(name, properties);
        }

        public static void ClearClasses()
        {
            initializedClasses.Clear();
        }
    }
}
