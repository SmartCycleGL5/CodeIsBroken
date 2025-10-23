using NaughtyAttributes;
using NUnit.Framework.Interfaces;
using SharpCube;
using SharpCube.Object;
using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
public class Script
{
    public string name;

    [Header("Code")]
    [ResizableTextArea] public string Code;
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

    public Script(string name, string code = null)
    {
        this.name = name;

        Compile(code == null ? DefaultCode : code);
    }

    public void Run()
    {
        //find start & update

        //Tick.OnTick += RunUpdate;
        //Tick.OnEndingTick += Stop;
    }
    public void Terminate()
    {

    }

    public void Compile(string code)
    {
        Debug.Log("try compile");
        classes.Clear();

        Code = code;

        Compiler.Interporate(this);

        foreach (var item in classes.@private)
        {
            name = item.Value.name;
            break;
        }

        Debug.Log("Finished compile");
    }

    internal void Clear()
    {
        throw new NotImplementedException();
    }
    
}
