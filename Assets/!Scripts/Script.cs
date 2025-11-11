using NaughtyAttributes;
using RoslynCSharp;
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
    public ScriptType type { get; private set; }
    string DefaultCode =>
            $"using UnityEngine;\n" +
            $"public class {name} : MonoBehaviour" +
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


    public Script(string name, string code = null, BaseMachine machine = null)
    {
        this.name = name;
        connectedMachine = machine;
        ScriptManager.instance.playerScripts.Add(name, this);

        Save(code == null ? DefaultCode : code);
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
        ScriptManager.Compile();
    }
    public bool Compile()
    {
        type = Domain.ScriptDomain.CompileAndLoadMainSource(rawCode);

        if (type != null)
        {
            return true;
        }

        return false;
    }

}
