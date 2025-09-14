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

    public void Print()
    {
        Debug.Log("this was called");
    }
    public void Move(int vla)
    {
        transform.position += Vector3.up * vla;
    }
    public void Rotate()
    {
        transform.Rotate(0, 5, 0);
    }
}
