using UnityEngine;

public class Saw : Machine
{
    protected override void Start()
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));
        base.Start();
    }
}
