using System;
using CodeIsBroken;
using ScriptEditor.Console;
using UnityEngine;

public abstract class Machine : MonoBehaviour
{
    public int tick;
    public bool active = true;
    
    private void Awake()
    {
        BaseMachine machine = GetComponent<BaseMachine>();
        machine.variableInfo.Clear();
        machine.methodInfo.Clear();
        machine.AddMethodsAsIntegrated(typeof(Machine));

        Tick.OnTick += UpdateVariables;
    }

    private void OnDestroy()
    {
        Tick.OnTick -= UpdateVariables;
    }

    public void SetActive(bool toggle)
    {
        enabled = toggle;
    }

    private void UpdateVariables()
    {
        tick = Tick.tickCount;
        active = enabled;
    }
}     