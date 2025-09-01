using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Class
{
    public string name;

    public Class inheritedClass;

    [SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, Dynamic> variables = new();

    public List<Method> methods;

    public string[] baseCode;

    public Class(string name, string[] baseCode)
    {
        this.baseCode = baseCode;
        this.name = name;
    }
    public Class(string name, List<string> baseCode)
    {
        this.baseCode = baseCode.ToArray();
        this.name = name;
    }
    public Class(string name, string[] baseCode, Class inheritedClass)
    {
        this.baseCode = baseCode;
        this.name = name;
        this.inheritedClass = inheritedClass;
    }
    public Class(string name, List<string> baseCode, Class inheritedClass)
    {
        this.baseCode = baseCode.ToArray();
        this.name = name;
        this.inheritedClass = inheritedClass;
    }
}
public class Method
{

}
