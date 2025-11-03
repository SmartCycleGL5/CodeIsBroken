using NaughtyAttributes;
using SharpCube;
using System;
using UnityEngine;
using WindowSystem;

[Serializable]
public class Script
{
    public string name;
    public BaseMachine connectedMachine;

    [Header("Code")]
    [field: SerializeField, ResizableTextArea] public string rawCode { get; private set; }
    string DefaultCode =>
            $"public class {name}" +
            "\n{" +

            "\n\tprivate void FirstTick()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t\t" +
            "\n\t\t" +
            "\n\t}" +

            "\n\t" +

            "\n\tprivate void OnTick()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t\t" +
            "\n\t\t" +
            "\n\t}" +

            "\n}";

    public Memory<Class> classes = new();

    public Script(string name, string code = null, BaseMachine machine = null)
    {
        this.name = name;
        connectedMachine = machine;
        ScriptManager.instance.playerScripts.Add(name, this);

        Save(code == null ? DefaultCode : code);
        Compile();
    }

    public void Run()
    {
        Debug.Log($"[Script {name}] running");
        //find start & update

        //Tick.OnTick += RunUpdate;
        //Tick.OnEndingTick += Stop;
    }
    public void Terminate()
    {

    }

    public void Edit()
    {
        Terminal.NewTerminal(this);
    }

    public void Save(string code)
    {
        rawCode = code;
    }
    public bool Compile()
    {
        Memory<Class> oldClasses = classes;
        classes.Clear();

        try
        {
            Compiler.StartCompile(this);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            PlayerConsole.LogError("Failed to compile", false);

            classes = oldClasses;

            return false;
        }
    }

    internal void Clear()
    {
        throw new NotImplementedException();
    }

}
