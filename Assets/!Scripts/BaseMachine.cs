using AYellowpaper.SerializedCollections;
using Coding;
using Coding.Language;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    [Button]
    void Initialize()
    {
        Initialize("NewClass");
    }
    public virtual void Initialize(string initialClassName)
    {
        if(Initialized)
        {
            Debug.LogError(initialClassName + " Already initialized!");
            return;
        }

        ScriptManager.instance.AddMachine(this);

        initialPos = transform.position;
        initialRot = transform.eulerAngles;

        //if(machineCode == null)
        //    machineCode = new MachineCode();

        Tick.OnStartingTick += RunStart;
        Tick.OnTick += RunUpdate;
        Tick.OnEndingTick += Stop;

        machineCode = new MachineCode(initialClassName, this);

        Initialized = true;
    }

    private void OnDestroy()
    {
        ScriptManager.instance.RemoveMachine(this);
    }

    public void RunStart()
    {
        isRunning = true;

        foreach (var Class in Classes)
        {
            try
            {
                start = Class.Value.FindMethod("Start");
            }
            catch (Exception e)
            {
                Debug.LogWarning("Start not found");
            }
        }

        if (start != null)
            start.TryRun();
    }
    public void RunUpdate()
    {
        foreach (var Class in Classes)
        {
            try
            {
                update = Class.Value.FindMethod("Update");
            }
            catch (Exception e)
            {
                Debug.LogWarning("Update not found");
            }
        }

        if (update != null)
            update.TryRun();
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
        Terminal.NewTerminal(this);
    }

    protected void AddMethodsAsIntegrated(System.Type machine)
    {
        foreach (var item in machine.GetMethods())
        {
            if (item.GetBaseDefinition() == item)
            {
                string name = item.Name;
                IntegratedMethods.Add(name, new IntegratedMethod(name, item.GetParameters(), item, this));
            }
        }
    }

    #region Deprecated
    public async Task Rotate(int amount)
    {
        float originalAmount = transform.eulerAngles.y;

        while ((originalAmount + amount) - transform.eulerAngles.y > .1f && isRunning)
        {
            transform.Rotate(0, amount * Time.deltaTime, 0);

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        if (isRunning)
            transform.eulerAngles = new Vector3(0, originalAmount + amount, 0);
    }
    public async Task Move(Vector3 dir)
    {
        Vector3 originalPos = transform.position;

        while (Vector3.Distance(originalPos + dir, transform.position) > .1f && isRunning)
        {
            transform.position += dir * Time.deltaTime;

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        if (isRunning)
            transform.position = originalPos + dir;
    }

    public async void Rocket()
    {
        while (isRunning)
        {
            _ = Rotate(360);
            await Move(Vector3.up);

            Debug.Log(isRunning);
        }
    }
    #endregion
}
