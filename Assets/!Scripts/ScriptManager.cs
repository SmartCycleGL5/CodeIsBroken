using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Collections.Generic;
using SharpCube.Object;
using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public SerializedDictionary<string, Script> playerScripts = new();
    public static ScriptManager instance;

    public static bool isRunning { get; private set; }

    public static List<BaseMachine> machines = new();

    private void Awake()
    {
        instance = this;
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
    public static void StartMachines()
    {
        if (isRunning) return;

        foreach (var script in instance.playerScripts)
        {
            script.Value.Run();
        }

        isRunning = true;

        Debug.Log("[ScriptManager] Starting");

        Tick.StartTick();
    }
    [Button]
    public static void StopMachines()
    {
        if (!isRunning) return;
        Tick.StopTick();

        Debug.Log("[ScriptManager] Ending");

        for (int i = Item.items.Count - 1; i >= 0; i--)
        {
            if (Item.items[i].destroyOnPause)
                Destroy(Item.items[i].gameObject);
        }

        isRunning = false;

    }

    public static void Re()
    {

        foreach (var item in machines)
        {
            item.ClearScripts();
        }
    }

    public void AddMachine(BaseMachine machine)
    {
        machines.Add(machine);
    }
    public void RemoveMachine(BaseMachine machine)
    {
        machines.Remove(machine);
    }
}