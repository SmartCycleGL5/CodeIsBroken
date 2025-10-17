using AYellowpaper.SerializedCollections;
using Coding;
using NaughtyAttributes;
using SharpCube;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MachineCode
{
    public string name;

    [Header("Code")]
    [ResizableTextArea] public string Code;

    [SerializedDictionary("Name", "Class")]
    public SerializedDictionary<string, Class> Classes = new();

    BaseMachine machine;

    public MachineCode(string name, BaseMachine machine)
    {
        this.name = name;
        this.machine = machine;
        CreateScript(name);
        Interporate.Classes(Code, machine);
    }
    public void Compile(string code)
    {
        Code = code;
        machine.ClearMemory();

        Compiler.Compile(this);

        foreach (var item in Classes)
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
