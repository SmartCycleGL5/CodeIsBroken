using AYellowpaper.SerializedCollections;
using WindowSystem;
using SharpCube;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(100), DisallowMultipleComponent]
public abstract class BaseMachine : MonoBehaviour
{
    public Dictionary<string, IntegratedMethod> IntegratedMethods = new();

    public List<Script> attachedScripts = new();

    public bool isRunning { get; private set; } = false;

    public bool Initialized { get; private set; } = false;

    [HideInInspector] public Terminal connectedTerminal;

    [Button]
    void AddScript()
    {
        AddScript(new Script("NewClass" + UnityEngine.Random.Range(1, 100)));
    }
    public virtual void AddScript(Script script)
    {
        attachedScripts.Add(script);
    }

    protected virtual void OnDestroy()
    {
        if (!Initialized) return;
        ScriptManager.instance.RemoveMachine(this);
    }

    void Run()
    {
        try
        {
            foreach (var script in attachedScripts)
            {
                script.Run();
            }
        }
        catch (Exception e)
        {
            ScriptManager.StopMachines();
            Debug.LogError("Couldnt run because of: \"" + e + "\"");
        }
    }

    public void Stop()
    {
        Application.quitting -= Stop;
        isRunning = false;

        //ResetThis();
    }

    public void ClearScripts()
    {
        foreach (var script in attachedScripts)
        {
            script.Clear();
        }
    }


    [Button]
    public void OpenTerminalForMachine()
    {
        if (connectedTerminal != null) return;
        Debug.Log("[BaseMachine] Open Terminal for " + this);
        connectedTerminal = Terminal.NewTerminal(this);
    }
    // Why is Torje breaking the code
    protected void AddMethodsAsIntegrated(System.Type machine)
    {
        foreach (var item in machine.GetMethods())
        {
            if (item.GetBaseDefinition() == item)
            {
                string name = item.Name;
                if (item.GetAttribute<DontIntegrate>() != null || item.IsSpecialName) continue;

                IntegratedMethods.Add(name, new IntegratedMethod(name, null, item.GetParameters(), item, this));
            }
        }
    }
}
