using AYellowpaper.SerializedCollections;
using CodeIsBroken.Audio;
using CodeIsBroken.Contract;
using CodeIsBroken.Product;

using FMODUnity;
using NaughtyAttributes;
using RoslynCSharp;
using ScriptEditor.Console;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static CodeIsBroken.UI.UIManager;

public class ScriptManager : MonoBehaviour
{
    public SerializedDictionary<string, Script> activePlayerScripts = new();
    public static ScriptManager instance;

    public static ScriptDomain scriptDomain;
    public static bool isRunning { get; private set; }
    public static Action<bool> onRun;
    
    static Button runButton;

    public static bool compiling;

    public static List<Error> compilerErrors = new List<Error>();

    [Header("Audio")]
    [SerializeField] EventReference powerUp;
    [SerializeField] EventReference powerDown;


    private void Awake()
    {
        instance = this;
        scriptDomain = new();
    }

    private void Start()
    {
        ContractManager.OnNewContracts += StopMachines;

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

        runButton.SetEnabled(false);
        runButton.text = "Starting";
        PlayerConsole.Clear();

        AudioManager.PlayOneShot(instance.powerUp, out int time);
        await Task.Delay(time / 2);

        foreach (var script in instance.activePlayerScripts)
        {
            script.Value.Run();
        }

        isRunning = true;

        Tick.StartTick();

        runButton.SetEnabled(true);
        runButton.text = "Stop";
    }
    [Button]
    public static async void StopMachines()
    {
        if(compiling) return;
        if (!isRunning) return;

        runButton.SetEnabled(false);
        runButton.text = "Stopping";
        Tick.StopTick();

        AudioManager.PlayOneShot(instance.powerDown, out int time);
        await Task.Delay(time / 2);

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

    public static async Task<bool> StartCompile()
    {
        if(compiling) return false;
        compiling = true;

        runButton.text = "Compiling...";
        runButton.SetEnabled(false);

        if (!await Compile())
        {
            runButton.text = "<color=#ff0000>Failed</color>";
            compiling = false;
            return false;
        }

        runButton.text = "Start";
        runButton.SetEnabled(true);

        compiling = false;
        return true;
    }

    static async Task<bool> Compile()
    {
        bool success = true;

        compilerErrors = new();

        foreach (var script in instance.activePlayerScripts)
        {
            if (!script.Value.Compile(ref compilerErrors))
                success = false;

            await Task.Delay(10);
        }

        PlayerConsole.Clear();

        if (!success)
        {
            foreach (var error in compilerErrors)
            {
                PlayerConsole.LogError(error.error.ToString(), error.source.name);
            }
        }

        return success;
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