using System;
using CodeIsBroken;
using ScriptEditor.Console;
using UnityEngine;

public abstract class Machine : MonoBehaviour
{
    private void Awake()
    {
        BaseMachine machine = GetComponent<BaseMachine>();
        machine.methodInfos.Clear();
        machine.AddMethodsAsIntegrated(typeof(Machine));
    }
    
    public virtual void Reset()
    {

    }
}     