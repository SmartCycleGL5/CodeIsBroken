using System;
using CodeIsBroken;
using ScriptEditor.Console;
using UnityEngine;

public abstract class Machine : MonoBehaviour
{
    public int tick;
    
    private void Awake()
    {
        BaseMachine machine = GetComponent<BaseMachine>();
        machine.methodInfo.Clear();
        machine.AddMethodsAsIntegrated(typeof(Machine));

        Tick.OnTick += UpdateVariables;
    }

    private void OnDestroy()
    {
        Tick.OnTick -= UpdateVariables;
    }

    private void UpdateVariables()
    {
        tick = Tick.tickCount;
    }

    public virtual void Reset()
    {

    }
}     