using NaughtyAttributes;
using RoslynCSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeIsBroken.Coding;
using Trivial.CodeSecurity;
using UnityEngine;

[Serializable]
public class Script
{
    public string name;
    public Programmable connectedMachine;

    public Action Deleted;

    [Header("Code")]
    public string rawCode { get; private set; }
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
        Compiler.activePlayerScripts.Add(name, this);

        Debug.Log(connectedMachine);

        _=Save(DefaultCode(className, parentClass));

        GameManager.onStart += Run;
        GameManager.onStop += Terminate;
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
        try
        {
            proxy.Methods.Call(startMethod);
        }
        catch
        (Exception ex)
        {
            Debug.LogWarning("No start method");
        }
    }
    void OnTick()
    {
        try
        {
            proxy.Methods.Call(updateMethod);
        }
        catch
        (Exception ex)
        {
            Debug.LogWarning("No start method");
        }
    }

    public async Task Save(string code)
    {
        rawCode = code;
        await Compiler.StartCompile();
    }
    public bool Compile(ref List<Error> errors)
    {
        type = Compiler.scriptDomain.CompileAndLoadMainSource(rawCode, out CompileResult compileResult, out CodeSecurityReport report);

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
        Compiler.activePlayerScripts.Remove(name);

        Deleted?.Invoke();
    }
}
