using NaughtyAttributes;
using System;
using Terminal;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MachineCode 
{
    [ResizableTextArea] public string Code;
    BaseMachine machine;
    public void Initialize(BaseMachine machine)
    {
        this.machine = machine;
        Interpreter.InterperateInitialization(Code, this.machine);
    }
    public void UpdateCode(string code)
    {
        Code = code;
        machine.ClearMemory();
        Interpreter.InterperateInitialization(Code, machine);
    }
    public void CreateScript(string name)
    {
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
