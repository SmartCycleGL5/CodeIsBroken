using Coding;
using NaughtyAttributes;
using RoslynCSharp;
using ScriptEditor;
using ScriptEditor.Console;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trivial.CodeSecurity;
using UnityEngine;

[Serializable]
public class Script
{
    public string name;
    public Programmable connectedMachine;

    Terminal terminal;

    public Action Deleted;

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

            "\n\t//Runs once on Start" +
            $"\n\tprivate void {startMethod}()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t}" +

            "\n\t" +

            "\n\t//Runs once every Tick/second" +
            $"\n\tprivate void {updateMethod}()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t}" +

            "\n}";
    }


    public  Script(string className, string parentClass, Programmable machine = null)
    {
        this.name = className;
        connectedMachine = machine;
        ScriptManager.instance.activePlayerScripts.Add(name, this);

        Debug.Log(connectedMachine);

        _=Save(DefaultCode(className, parentClass));
    }

    public void Run()
    {        
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
        if (terminal != null) return;
        terminal = Terminal.NewTerminal(this, connectedMachine);
    }

    public async Task Save(string code)
    {
        rawCode = code;
        await ScriptManager.StartCompile();
    }
    public bool Compile(ref List<Error> errors)
    {
        type = ScriptManager.scriptDomain.CompileAndLoadMainSource(rawCode, out CompileResult compileResult, out CodeSecurityReport report);

        if (proxy != null) proxy.Dispose();

        if (!compileResult.Success)
        {
            foreach (CompileError error in compileResult.Errors)
            {
                errors.Add(new(this, error));
            }
            return false;
        }

        if (connectedMachine != null)
            proxy = type.CreateInstance(connectedMachine.gameObject);
        else
            proxy = type.CreateInstance();

        return true;
    }
    
    public void Delete()
    {
        ScriptManager.instance.activePlayerScripts.Remove(name);

        Deleted?.Invoke();
    }
}
