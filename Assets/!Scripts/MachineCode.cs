using Mono.Cecil.Cil;
using NaughtyAttributes;
using System;
using Coding;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MachineCode 
{
    public string name;
    [ResizableTextArea] public string Code;
    BaseMachine machine;
    public void Initialize(string name, BaseMachine machine)
    {
        this.name = name;
        this.machine = machine;
        Interpreter.InterperateScript(Code, this.machine);
    }
    public void UpdateCode(string code)
    {
        Code = code;
        machine.ClearMemory();
        Interpreter.InterperateScript(Code, machine);
    }
    public void CreateScript(string name)
    {
        this.name = name;
        Code =
                "class " + name + "\n" +
                "{\n" +
                "\tvoid Start()\n" +
                "\t{\n" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t\t\n" +
                "\t}\n" +
                "}";
    }
}
