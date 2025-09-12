using NaughtyAttributes;
using System;
using Terminal;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MachineCode 
{
    [ResizableTextArea] public string Code;

    public void Initialize(ref MachineScript machine)
    {
        Interpreter.InterperateInitialization(Code, ref machine);
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
                "\n\t\t" +
                "\t}\n" +
                "}";
    }
}
