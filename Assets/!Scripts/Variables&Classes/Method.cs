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
        foreach (var item in methodCode)
        {
            Code.Add(new CodeSnippet(item, this));
        }
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
        Rotate5,
        Rotate15,
        MoveUp,
        MoveDown,
        MoveRight,
        MoveLeft,
        Rocket,
    }
    public ToRun code;
    Method method;

    public CodeSnippet(string snippet, Method method)
    {
        switch (snippet)
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
            case nameof(ToRun.Rotate5) + "()":
                {
                    code = ToRun.Rotate5;
                    break;
                }
            case nameof(ToRun.Rotate15) + "()":
                {
                    code = ToRun.Rotate15;
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
            case nameof(ToRun.Rocket) + "()":
                {
                    code = ToRun.Rocket;
                    break;
                }

        }

        this.method = method;
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
            case ToRun.Rotate5:
                {
                    Rotate(5);
                    break;
                }
            case ToRun.Rotate15:
                {
                    Rotate(15);
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
            case ToRun.Rocket:
                {
                    Rocket();
                    break;
                }
        }
    }

    public void Reset()
    {
        method.Class.machine.ResetThis();
    }
    public void Rotate(int amount)
    {
        _ = method.Class.machine.Rotate(amount);
    }
    public void Move(Vector3 dir)
    {
        _= method.Class.machine.Move(dir);
    }
    public void Rocket()
    {
        method.Class.machine.Rocket();
    }
}
