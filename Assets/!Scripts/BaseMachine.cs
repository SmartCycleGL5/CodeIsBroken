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

    protected virtual void Start()
    {
        machineCode.Initialize(name, this);
        ScriptManager.instance.AddMachine(this);
        machineCode.Initialize(this);

        initialPos = transform.position;
        initialRot = transform.eulerAngles;

        if(machineCode == null)
            machineCode = new MachineCode();
    }

    private void OnDestroy()
    {
        ScriptManager.instance.RemoveMachine(this);
    }

    public void Run()
    {
        isRunning = true;

        Application.quitting += Stop;

        foreach (var Class in Classes)
        {
            Class.Value.FindMethod("Start").TryRun();
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
