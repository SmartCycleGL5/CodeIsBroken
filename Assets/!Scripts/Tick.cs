using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tick : MonoBehaviour
{
    public static Tick Instance { get; private set; }
    [SerializeField] private float tickTime = 0.5f;
    public static float tickLength => Instance.tickTime;
    public static int tick;

    public static event Action OnTick;

    public static event Action OnLateTick;
    public static event Action OnStartingTick;
    public static event Action OnEndingTick;

    // Always Active tick
    public static Action OnGameTick;
    
    
    private void Awake()
    {
        tick = 0;
        Instance = this;

    }


    public static void StartTick()
    {
        tick = 0;
        OnStartingTick?.Invoke();
        Instance.InvokeRepeating(nameof(DoTick), 0, tickLength);

        Application.quitting += StopTick;

    }
    public static void StopTick()
    {
        OnEndingTick?.Invoke();
        Instance.CancelInvoke(nameof(DoTick));

        Application.quitting -= StopTick;


    }

    public static void GameTick()
    {
        OnGameTick?.Invoke();
    }

    void DoTick()
    {
        //Debug.Log("Tick bro");
        tick++;
        OnTick?.Invoke();
        OnLateTick?.Invoke();
    }

}
