using System;
using System.Collections.Generic;
using UnityEngine;
using Coding.SharpCube;
using System.Reflection;

public class Machine : BaseMachine
{
    protected virtual void Start()
    {
        Tick.OnEndingTick += Reset;
        AddMethodsAsIntegrated(typeof(Machine));
    }

    public void Print(string debug)
    {
        Debug.Log(debug);
    }

    public virtual void Reset()
    {

    }
    
    protected virtual void OnDestroy()
    {
        base.OnDestroy();
        Tick.OnEndingTick -= Reset;
    }
}
