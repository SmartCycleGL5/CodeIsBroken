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
    public static Action<bool> onRun;
    
    static Button  runButton;

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

        await StartCompile();

        onRun?.Invoke(true);

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

        onRun?.Invoke(false);

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

    public static async Task StartCompile()
    {
        if(compiling) return;
        compiling = true;
        
        runButton.SetEnabled(false);

        await Compile();
        
        runButton.SetEnabled(true);
        compiling = false;
    }

    static async Task Compile()
    {
        bool success = true;

        List<Error> errors = new();

        foreach (var script in instance.activePlayerScripts)
        {
            if (!script.Value.Compile(ref errors))
                success = false;

            await Task.Delay(10);
        }

        PlayerConsole.Clear();

        if (!success)
        {
            foreach (var error in errors)
            {
                PlayerConsole.LogError(error.error.ToString(), error.source.name);
            }
        }
    }

    private void OnDestroy()
    {
        StopMachines();
    }
}

public struct Error
{
    public Script source;
    public CompileError error;

    public Error (Script source, CompileError error)
    {
        this.source = source;
        this.error = error;
    }
}