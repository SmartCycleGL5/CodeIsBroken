using System;
using System.Collections.Generic;
using UnityEngine;

public class Machine : BaseMachine
{
    protected override void Start()
    {
        Debug.Log(nameof(Print) + "()");
        IntegratedMethods.Add(nameof(Print) + "()", Print);

        base.Start();
    }

    public void Print()
    {
        Debug.Log("this was called");
    }
}
