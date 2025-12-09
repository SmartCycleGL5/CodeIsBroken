using FMODUnity;
using NaughtyAttributes;
using System;
using System.Threading.Tasks;
using CodeIsBroken.Audio;
using CodeIsBroken.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public static bool isRunning { get; private set; }
    public static Action onStart;
    public static Action onStop;
    
    public static Button runButton { get; private set; }

    [Header("Audio")]
    [SerializeField] EventReference powerUp;
    [SerializeField] EventReference powerDown;


    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        runButton = UIManager.canvas.Q<Button>("Run");
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
        if (isRunning) return;

        runButton.SetEnabled(false);
        runButton.text = "Starting";
        //PlayerConsole.Clear();

        AudioManager.PlayOneShot(instance.powerUp, out int time);
        await Task.Delay(time / 2);

        onStart?.Invoke();

        isRunning = true;

        Tick.StartTick();

        runButton.SetEnabled(true);
        runButton.text = "Stop";
    }
    [Button]
    public static async void StopMachines()
    {
        if (!isRunning) return;

        runButton.SetEnabled(false);
        runButton.text = "Stopping";
        Tick.StopTick();

        AudioManager.PlayOneShot(instance.powerDown, out int time);
        await Task.Delay(time / 2);

        onStop?.Invoke();

        isRunning = false;

        runButton.SetEnabled(true);
        runButton.text = "Start";
    }

    private void OnDestroy()
    {
        StopMachines();
    }
}