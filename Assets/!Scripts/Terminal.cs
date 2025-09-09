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
    static List<Action> Start = new();

    public static void AddStart(Action action)
    {
        Start.Add(action);
        OnStart += action;
    }

    public void Initialize()
    {
        machine.ClearMemory();

        foreach (var item in Start)
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
