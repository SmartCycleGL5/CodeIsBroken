using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Class
{
    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, Dynamic> variables;

    public List<Method> methods;

    public string[] baseCode;

    public Class(string[] baseCode)
    {
        this.baseCode = baseCode;
    }
    public Class(List<string> baseCode)
    {
        this.baseCode = baseCode.ToArray();
    }
}
public class Method
{

}
