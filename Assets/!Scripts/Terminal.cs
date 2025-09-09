using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public TMP_InputField input;

    public MachineScript machine;
    public static event Action OnStart;
    static List<Action> Starting = new();

    public static void AddStart(Action action)
    {
        Starting.Add(action);
        OnStart += action;
    }

    public void Initialize()
    {
        machine.ClearMemory();

        foreach (var item in Starting)
        {
            OnStart -= item;
        }

        Interpreter.InterperateInitialization(input.text, ref machine);
    }

    public void Run()
    {
        OnStart?.Invoke();
    }
}
