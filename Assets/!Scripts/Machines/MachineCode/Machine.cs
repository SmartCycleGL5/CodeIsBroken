using System;
using CodeIsBroken;
using ScriptEditor.Console;
using UnityEngine;

public abstract class Machine : MonoBehaviour
{
    public int tick;
    public bool active = true;
    
    private void OnEnable()
    {
        Programmable machine = GetComponent<Programmable>();
        machine.variableInfo.Clear();
        machine.methodInfo.Clear();
        machine.AddMethodsAsIntegrated(typeof(Machine));

        Tick.OnTick += UpdateVariables;
    }
     void OnDisable()
    {
        Tick.OnTick -= UpdateVariables;
    }

    public void SetActive(bool toggle)
    {
        enabled = toggle;
    }

    private void UpdateVariables()
    {
        Debug.Log(this);
        tick = Tick.tickCount;
        active = enabled;
    }
}     