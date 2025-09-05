using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[Serializable]
public class Method : IVariable
{
    public string name;

    public Type[] requiredInputTypes = new Type[0];
    public Type returnType;
    [HideInInspector] public Class Class;

    [field: SerializeField, SerializedDictionary("Name", "Value")]
    public SerializedDictionary<string, Variable> variables { get; set; } = new();

    public string[] methodCode;
    public Stack<CodeSnippet> Code;

    public Method(string name, Type returnType, string[] methodCode, Class @class)
    {
        this.name = name;
        this.returnType = returnType;
        this.methodCode = methodCode;
        Class = @class;

        InitializeMethod();
    }
    public void InitializeMethod()
    {
        if (name == "Start()")
        {
            Terminal.OnStart += TryRun;
            Debug.Log("Start Found");
        }
    }

    /// <summary>
    /// Try to run the method
    /// </summary>
    /// <param name="input">the input variables</param>
    /// <returns>returns true if it was successful</returns>
    void TryRun()
    {
        TryRun(null);
    }
    /// <summary>
    /// Try to run the method
    /// </summary>
    /// <param name="input">the input variables</param>
    /// <returns>returns true if it was successful</returns>
    public bool TryRun(Variable[] input = null)
    {
        if (input == null)
        {
            input = new Variable[0];
        }
        if (input.Length == requiredInputTypes.Length)
        {
            for (int i = 0; i < requiredInputTypes.Length; i++)
            {
                if(input[i].type != requiredInputTypes[i])
                {
                    variables.Clear();
                    return false;
                }

                NewVariable(input[i]);
            }

            Run();
            return true;
        }

        return false;
    }
    void Run()
    {
        foreach (var item in methodCode)
        {
            Debug.Log(item);
        }

        //foreach (var snippet in Code)
        //{
        //    snippet.Run();
        //}

        //variables.Clear();
    }
    public Variable NewVariable(string name, Type Type = Type.Bool)
    {
        Variable value = new Variable(name, Type);
        variables.Add(name, value);
        return value;
    }
    public Variable NewVariable(Variable variable)
    {
        variables.Add(variable.name, variable);
        return variable;
    }
    public Variable FindVariable(string name)
    {
        if (variables[name] != null)
        {
            return variables[name];
        }
        else if (Class.variables[name] != null)
        {
            return Class.FindVariable(name);
        }

        return null;
    }

}

[Serializable]
public class CodeSnippet
{
    public void Run()
    {

    }
}
