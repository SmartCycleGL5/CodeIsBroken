using System;
using UnityEngine;
using UnityEngine.EventSystems;
public class Tick : MonoBehaviour
{
    [SerializeField] private float tickTimerMax = 0.5f;
    private int tick;
    private float tickTimer;
    private bool tickTimerActive;
    
    public static event Action OnTick;
    
    private void Awake()
    {
        tick = 0;
    }

    private void Update()
    {
        tickTimer += Time.deltaTime;
        if (tickTimer >= tickTimerMax)
        {
            Debug.Log(tick);
            tickTimer = 0;
            tick++;
            OnTick?.Invoke();
        }
    }
}
