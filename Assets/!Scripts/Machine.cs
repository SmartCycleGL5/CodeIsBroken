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
    }
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(Machine));

        base.Initialize(initialClassName);
    }

    public void Print(string debug)
    {
        Debug.Log(debug);
    }

    public virtual void Reset()
    {

    }
    
}
