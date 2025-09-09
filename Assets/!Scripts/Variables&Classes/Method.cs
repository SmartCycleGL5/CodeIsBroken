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
    public List<CodeSnippet> Code = new();

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
            Terminal.AddStart(TryRun);
            Debug.Log("Start Found");
        }

        foreach (var item in methodCode)
        {
            Code.Add(new CodeSnippet(item));
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

        foreach (var snippet in Code)
        {
            snippet.Run();
        }

        variables.Clear();
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
/// <summary>
/// will be removed, just a temporary solution
/// </summary>
[Serializable]
public class CodeSnippet
{
    public enum ToRun
    {
        None,
        Reset,
        Rotate45,
        Rotate180,
        Rotate270,
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
    }
    public ToRun code;

    public CodeSnippet(string snippet)
    {
        switch(snippet)
        {
            case nameof(ToRun.Reset) + "()":
                {
                    code = ToRun.Reset;
                    break;
                }
            case nameof(ToRun.Rotate45) + "()":
                {
                    code = ToRun.Rotate45;
                    break;
                }
            case nameof(ToRun.Rotate180) + "()":
                {
                    code = ToRun.Rotate180;
                    break;
                }
            case nameof(ToRun.Rotate270) + "()":
                {
                    code = ToRun.Rotate270;
                    break;
                }
            case nameof(ToRun.MoveUp) + "()":
                {
                    code = ToRun.MoveUp;
                    break;
                }
            case nameof(ToRun.MoveDown) + "()":
                {
                    code = ToRun.MoveDown;
                    break;
                }
            case nameof(ToRun.MoveRight) + "()":
                {
                    code = ToRun.MoveRight;
                    break;
                }
            case nameof(ToRun.MoveLeft) + "()":
                {
                    code = ToRun.MoveLeft;
                    break;
                }

        }
    }

    public void Run()
    {
        Debug.Log(code);

        switch (code)
        {
            case ToRun.Reset:
                {
                    Reset();
                    break;
                }
            case ToRun.Rotate45:
                {
                    Rotate(45);
                    break;
                }
            case ToRun.Rotate180:
                {
                    Rotate(180);
                    break;
                }
            case ToRun.Rotate270:
                {
                    Rotate(270);
                    break;
                }
            case ToRun.MoveUp:
                {
                    Move(Vector3.up);
                    break;
                }
            case ToRun.MoveDown:
                {
                    Move(Vector3.down);
                    break;
                }
            case ToRun.MoveRight:
                {
                    Move(Vector3.right);
                    break;
                }
            case ToRun.MoveLeft:
                {
                    Move(Vector3.left);
                    break;
                }
        }
    }

    public void Reset()
    {
        MachineScript.Reset();
    }
    public void Rotate(int amount)
    {
        MachineScript.Rotate(amount);
    }
    public void Move(Vector3 dir)
    {
        MachineScript.Move(dir);
    }
}
