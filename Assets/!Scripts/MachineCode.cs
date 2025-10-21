using NaughtyAttributes;
using SharpCube;
using SharpCube.keyword;
using System;
using UnityEngine;

[Serializable]
public class MachineCode
{
    public string name;

    [Header("Code")]
    [ResizableTextArea] public string Code;

    public Memory<@class> classes;

    BaseMachine machine;

    public MachineCode(string name, BaseMachine machine)
    {
        this.name = name;
        this.machine = machine;
        CreateScript(name);
    }
    public void Compile(string code)
    {
        Code = code;
        machine.ClearMemory();

        Compiler.Compile(this);

        foreach (var item in classes.@private)
        {
            name = item.Value.name;
            break;
        }
    }
    void CreateScript(string name)
    {
        Code =
                "class " + name + "" +
                "\n{" +

                "\n\tvoid FirstTick()" +
                "\n\t{" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t}" +

                "\n\t" +

                "\n\tvoid OnTick()" +
                "\n\t{" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t}" +

                "\n}";
    }
}
