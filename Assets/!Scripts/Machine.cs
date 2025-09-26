using System;
using System.Collections.Generic;
using UnityEngine;
using Coding.Language;
using System.Reflection;

public class Machine : BaseMachine
{
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(Machine));

        base.Initialize(initialClassName);
    }

    public void Print(string debug)
    {
        Debug.Log(debug);
    }

}
