using System;
using System.Collections.Generic;
using UnityEngine;
using Coding.Language;
using System.Reflection;

public class Machine : BaseMachine
{
    protected override void Start()
    {
        AddMethodsAsIntegrated(typeof(Machine));

        base.Start();
    }

    public void Print(string debug)
    {
        Debug.Log(debug);
    }
    public void Move(float amount)
    {
        transform.position += Vector3.up * amount;
    }
    public void Rotate()
    {
        transform.Rotate(0, 5, 0);
    }
}
