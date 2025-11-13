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

    private static string startMethod => "StartTick";
    private static string updateMethod => "OnTick";
    
    static string DefaultCode(string className, string parentClass)
    {
        return
            $"using CodeIsBroken;\n\n" +
            $"public class {className} : {parentClass}" +
            "\n{" +

            $"\n\tprivate void {startMethod}()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t}" +

            "\n\t" +

            $"\n\tprivate void {updateMethod}()" +
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

        Debug.Log(connectedMachine);

        Save(DefaultCode(className, parentClass));
    }

    public void Run()
    {
        ScriptManager.Compile();
        
        Tick.OnStartingTick += StartTick;
        Tick.OnTick += OnTick;
    }
    public void Terminate()
    {
        Tick.OnStartingTick -= StartTick;
        Tick.OnTick -= OnTick;
    }

    void StartTick()
    {
        proxy.Methods.Call(startMethod);
    }
    void OnTick()
    {
        proxy.Methods.Call(updateMethod);
    }
    
    public void Edit()
    {
        Terminal.NewTerminal(this, connectedMachine);
    }

    public void Save(string code)
    {
        rawCode = code;

        ScriptManager.Compile();

        PlayerConsole.Log("Saved!");
    }
    public bool Compile()
    {
        type = ScriptManager.scriptDomain.CompileAndLoadMainSource(rawCode, out CompileResult compileResult, out CodeSecurityReport report);
        
        if (!compileResult.Success)
        {
            foreach (var error in compileResult.Errors)
            {
                PlayerConsole.LogError(error);
            }
            return false;
        }

        Debug.Log(connectedMachine);

        if (proxy != null) proxy.Dispose();

        if (connectedMachine != null)
            proxy = type.CreateInstance(connectedMachine.gameObject);
        else
            proxy = type.CreateInstance();

        return true;
    }
    

}
