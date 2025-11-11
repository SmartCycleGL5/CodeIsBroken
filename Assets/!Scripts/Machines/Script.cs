using NaughtyAttributes;
using RoslynCSharp;
using ScriptEditor;
using ScriptEditor.Console;
using System;
using Trivial.CodeSecurity;
using UnityEngine;

[Serializable]
public class Script
{
    public string name;
    public BaseMachine connectedMachine;

    [Header("Code")]
    [field: SerializeField, ResizableTextArea] public string rawCode { get; private set; }
    public ScriptType type { get; private set; }
    public ScriptProxy proxy { get; private set; }
    static string DefaultCode(string className, string parentClass)
    {
        return
            $"using Machines;\n\n" +
            $"public class {className} : {parentClass}" +
            "\n{" +

            "\n\tprivate void StartTick()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t}" +

            "\n\t" +

            "\n\tprivate void OnTick()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t}" +

            "\n}";
    }


    public Script(string className, string parentClass, BaseMachine machine = null)
    {
        this.name = className;
        connectedMachine = machine;
        ScriptManager.instance.playerScripts.Add(name, this);

        Save(DefaultCode(className, parentClass));
    }

    public void Run()
    {
        ScriptManager.Compile();
        
        proxy = type.CreateInstance(connectedMachine.gameObject);

        Tick.OnStartingTick += StartTick;
        Tick.OnTick += OnTick;
    }

    void StartTick()
    {
        proxy.Methods.Call("StartTick");
    }
    void OnTick()
    {
        proxy.Methods.Call("OnTick");
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
        
        try
        {
            ScriptManager.Compile();
            PlayerConsole.Log("Saved!");
        }
        catch 
        {
            PlayerConsole.LogError("Failed to compile");
        }
    }
    public void Compile()
    {
        type = ScriptManager.scriptDomain.CompileAndLoadMainSource(rawCode, out CompileResult compileResult, out CodeSecurityReport report);

        if (!compileResult.Success)
        {
            foreach (var error in compileResult.Errors)
            {
                PlayerConsole.LogError(error);
            }
            throw new Exception();
        } 
    }
    

}
