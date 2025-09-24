using UnityEngine;

public class Saw : Machine
{
    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));

        base.Initialize(initialClassName);
    }




}
