using System;
using System.CodeDom.Compiler;
using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using RoslynCSharp;
using System.Collections.Generic;
using UnityEngine;
using ScriptEditor.Console;
using System.Threading.Tasks;
using CodeIsBroken.Product;
using UnityEngine.UIElements;
using static CodeIsBroken.UI.UIManager;

public class ScriptManager : MonoBehaviour
{
    public SerializedDictionary<string, Script> activePlayerScripts = new();
    public static ScriptManager instance;

    public static ScriptDomain scriptDomain;
    public static bool isRunning { get; private set; }
    
    static Button  runButton;

    public static List<Programmable> machines = new();

    public static bool compiling;

    private void Awake()
    {
        instance = this;
        scriptDomain = new();
    }

    private void Start()
    {
        runButton = canvas.Q<Button>("Run");
        runButton.clicked += ToggleMachines;
        runButton.text = "Start";
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
        if(compiling) return;
        if (isRunning) return;
        
        runButton.text = "Starting";
        runButton.SetEnabled(false);
        PlayerConsole.Clear();

        await Compile();

        foreach (var script in instance.activePlayerScripts)
        {
            script.Value.Run();
        }

        isRunning = true;

        Debug.Log("[ScriptManager] Starting");

        await Task.Delay(1000);

        Tick.StartTick();
        
        runButton.SetEnabled(true);
        runButton.text = "Stop";
    }
    [Button]
    public static void StopMachines()
    {
        if(compiling) return;
        if (!isRunning) return;
        
        runButton.text = "Stopping";
        runButton.SetEnabled(false);
        Tick.StopTick();

        Debug.Log("[ScriptManager] Ending");

        foreach (var script in instance.activePlayerScripts)
        {
            script.Value.Terminate();
        }

        for (int i = Item.items.Count - 1; i >= 0; i--)
        {
            Destroy(Item.items[i].gameObject);
        }

        isRunning = false;
        runButton.SetEnabled(true);
        runButton.text = "Start";
    }

    public void AddMachine(Programmable machine)
    {
        machines.Add(machine);
    }
    public void RemoveMachine(Programmable machine)
    {
        machines.Remove(machine);
    }

    public static async Task Compile()
    {
        if(compiling) return;
        compiling = true;
        
        runButton.SetEnabled(false);
        bool success = true;
        
        List<CompileError> errors = new();
        
        foreach (var script in instance.activePlayerScripts)
        {
            if(!script.Value.Compile(ref errors))
                success = false;
            
            await Task.Delay(10);
        }
        
        PlayerConsole.Clear();

        if (!success)
        {
            foreach (var error in errors)
            {
                PlayerConsole.LogError(error.ToString());
            }
        }
        
        runButton.SetEnabled(true);
        compiling = false;
    }

    private void OnDestroy()
    {
        StopMachines();
    }
}