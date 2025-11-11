using AYellowpaper.SerializedCollections;
using WindowSystem;
using SharpCube;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(100), DisallowMultipleComponent]
public class BaseMachine : MonoBehaviour
{
    public Dictionary<string, IntegratedMethod> IntegratedMethods = new();

    public List<Script> attachedScripts = new();


    public bool Initialized { get; private set; } = false;

    [Button]
    void AddScript()
    {
        AddScript(new Script("NewClass" + UnityEngine.Random.Range(1, 100)));
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
