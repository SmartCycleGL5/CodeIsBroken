using AYellowpaper.SerializedCollections;
using Coding;
using Coding.SharpCube;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(100), DisallowMultipleComponent]
public abstract class BaseMachine : MonoBehaviour
{
    public MachineCode machineCode;

    [SerializedDictionary("Name", "Class")]
    public SerializedDictionary<string, Class> Classes = new();

    public bool isRunning { get; private set; } = false;

    Vector3 initialPos;
    Vector3 initialRot;

    public Dictionary<string, IntegratedMethod> IntegratedMethods = new();

    Method start;
    Method update;

    public bool Initialized { get; private set; } = false;

    public Terminal connectedTerminal;

    [Button]
    void Initialize()
    {
        Initialize("NewClass" + UnityEngine.Random.Range(1, 100));
    }
    public virtual void Initialize(string initialClassName)
    {
        Debug.LogError("[BaseMachine] initialize");

        if (Initialized)
        {
            Debug.LogError("[BaseMachine]" + initialClassName + " Already initialized!");
            return;
        }

        ScriptManager.instance.AddMachine(this);

        initialPos = transform.position;
        initialRot = transform.eulerAngles;

        //if(machineCode == null)
        //    machineCode = new MachineCode();

        Tick.OnStartingTick += FindStartAndUpdate;
        Tick.OnStartingTick += RunStart;
        Tick.OnTick += RunUpdate;
        Tick.OnEndingTick += Stop;

        machineCode = new MachineCode(initialClassName, this);

        Initialized = true;
    }

    private void OnDestroy()
    {
        if (!Initialized) return;
        ScriptManager.instance.RemoveMachine(this);
        Tick.OnStartingTick -= FindStartAndUpdate;
        Tick.OnStartingTick -= RunStart;
        Tick.OnTick -= RunUpdate;
        Tick.OnEndingTick -= Stop;
    }
    void FindStartAndUpdate()
    {
        foreach (var Class in Classes)
        {
            try
            {
                start = Class.Value.GetMethod("Start");
            }
            catch
            {
                Debug.LogWarning("Start not found");
            }

            try
            {
                update = Class.Value.GetMethod("Update");
            }
            catch (Exception e)
            {
                Debug.LogWarning("Update not found " + e);
                return;
            }
        }
    }
    public void RunStart()
    {
        isRunning = true;

        if (update != null)
            Run(start);
    }
    public void RunUpdate()
    {
        if (update != null)
            Run(update);
    }
    void Run(Method toRun)
    {
        try
        {
            toRun.TryRun();
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

    public void ClearMemory()
    {
        Classes.Clear();
    }


    public void ResetThis()
    {
        transform.position = initialPos;
        transform.eulerAngles = initialRot;
    }

    [Button]
    public void OpenTerminalForMachine()
    {
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
