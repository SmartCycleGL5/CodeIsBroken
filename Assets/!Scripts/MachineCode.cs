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
    public async void UpdateCode(string code)
    {
        Code = code;
        machine.ClearMemory();

        await Task.Delay(1000);

        Interporate.Classes(Code, machine);
    }
    void CreateScript(string name)
    {
        Code =
                "class " + name + "" +
                "\n{" +
                "\n\tvoid Start()" +
                "\n\t{" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t}" +
                "\n" +

                "\n\tvoid Update()" +
                "\n\t{" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t\t" +
                "\n\t}" +
                "\n}";
    }
}
