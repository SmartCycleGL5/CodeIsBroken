using System;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using RoslynCSharp;
using System.Collections.Generic;
using UnityEngine;
using ScriptEditor.Console;
using System.Threading.Tasks;
public class ScriptManager : MonoBehaviour
{
    public SerializedDictionary<string, Script> activePlayerScripts = new();
    public static ScriptManager instance;

    public static ScriptDomain scriptDomain;
    public static bool isRunning { get; private set; }

    public static List<BaseMachine> machines = new();

    private void Awake()
    {
        instance = this;
        scriptDomain = new();
    }

    public static void ToggleMachines()
    {
        if (isRunning)
        {
            StopMachines();
        }
        else
        {
            StartMachines();
        }
    }

    [Button]
    public static async void StartMachines()
    {
        if (isRunning) return;
        PlayerConsole.Clear();

        Compile();

        foreach (var script in instance.activePlayerScripts)
        {
            script.Value.Run();
        }

        isRunning = true;

        Debug.Log("[ScriptManager] Starting");

        await Task.Delay(1000);

        Tick.StartTick();
    }
    [Button]
    public static void StopMachines()
    {
        if (!isRunning) return;
        Tick.StopTick();

        Debug.Log("[ScriptManager] Ending");

        foreach (var script in instance.activePlayerScripts)
        {
            script.Value.Terminate();
        }

        for (int i = Item.items.Count - 1; i >= 0; i--)
        {
            if (Item.items[i].destroyOnPause)
                Destroy(Item.items[i].gameObject);
        }

        isRunning = false;

    }

    public void AddMachine(BaseMachine machine)
    {
        machines.Add(machine);
    }
    public void RemoveMachine(BaseMachine machine)
    {
        machines.Remove(machine);
    }

    public static void Compile()
    {
        bool success = true;
        
        foreach (var script in instance.activePlayerScripts)
        {
            if(!script.Value.Compile())
                success = false;
        }

        if(!success)
            PlayerConsole.LogError("Failed to compile!");
    }

    private void OnDestroy()
    {
        StopMachines();
    }
}