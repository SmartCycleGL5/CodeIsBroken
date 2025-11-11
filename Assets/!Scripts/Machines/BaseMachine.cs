using NaughtyAttributes;
using SharpCube;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100), DisallowMultipleComponent]
public class BaseMachine : MonoBehaviour
{
    [InfoBox("The name of the monobehaviour class")]
    public string toDeriveFrom;
    public Dictionary<string, IntegratedMethod> IntegratedMethods = new();

    public List<Script> attachedScripts = new();


    public bool Initialized { get; private set; } = false;

    [Button]
    public void AddScript()
    {
        AddScript(new Script("NewScript" + UnityEngine.Random.Range(1, 100), toDeriveFrom));
    }
    public virtual void AddScript(Script script)
    {
        script.connectedMachine = this;
        attachedScripts.Add(script);
    }

    protected virtual void OnDestroy()
    {
        if (!Initialized) return;
        ScriptManager.instance.RemoveMachine(this);
    }


    [Button]
    public void OpenTerminalForMachine(int script = 0)
    {
        attachedScripts[script].Edit();
    }
    // Why is Torje breaking the code
}
