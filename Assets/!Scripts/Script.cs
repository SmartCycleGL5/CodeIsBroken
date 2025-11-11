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
    string DefaultCode(string className, string parentClass)
    {
        return
            $"public class {className} : {parentClass}" +
            "\n{" +

            "\n\tprivate void Start()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t\t" +
            "\n\t\t" +
            "\n\t}" +

            "\n\t" +

            "\n\tprivate void Update()" +
            "\n\t{" +
            "\n\t\t" +
            "\n\t\t" +
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
        ScriptProxy proxy = type.CreateInstance(connectedMachine.gameObject);

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
        }
        catch 
        {
            PlayerConsole.LogError("Failed to compile");
        }
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
        } else
        {
            return true;
        }
    }

}
