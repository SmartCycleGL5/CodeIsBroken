using NaughtyAttributes;
using System;
using Coding;
using Unity.VisualScripting;
using UnityEngine;
using System.Threading.Tasks;

[Serializable]
public class MachineCode 
{
    public string name;
    [ResizableTextArea] public string Code;
    BaseMachine machine;

    public MachineCode(string name, BaseMachine machine)
    {
        this.name = name;
        this.machine = machine;
        CreateScript(name);
        Interporate.Classes(Code, machine);
    }
    public void UpdateCode(string code)
    {
        Code = code;
        machine.ClearMemory();

        Interporate.Classes(Code, machine);

        foreach (var item in machine.Classes)
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
                "\n" +

                "\n\tvoid OnTick()" +
                "\n\t{" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t}" +
                "\n}";
    }
}
