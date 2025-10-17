using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class Metrics : MonoBehaviour
{
    public static Metrics instance;
    
    private int totalElectricity;
    private int timeSinceLevelUp;
    [SerializedDictionary("Level", "Electricity")]
    public SerializedDictionary<int, int> electricityLevel = new();
    [SerializedDictionary("Level", "Time")]
    public SerializedDictionary<int, int> timeUsed = new();

    private void Start()
    {
        instance = this;
        PlayerProgression.onLevelUp += TimeLevelUp;
        PlayerProgression.onLevelUp += ElectricityLevelUp;
    }

    // Used for machines to add electricity,
    public void UseElectricity(int increase)
    {
        totalElectricity += increase;
    }

    // Adds electricity to metrics on level up
    private void ElectricityLevelUp(int level)
    {
        electricityLevel.Add(level, totalElectricity);
        totalElectricity = 0;
    }

    // Adds time to metrics on level up
    private void TimeLevelUp(int level)
    {
        int time = (int)Time.time;
        timeUsed.Add(level, time - timeSinceLevelUp);
        timeSinceLevelUp = time;
    }

    
    

    public void GenerateGraph()
    {
        // Graph for electricity and time.
        // UI-Toolkit
    }

    private void OnDestroy()
    {
        PlayerProgression.onLevelUp -= TimeLevelUp;
        PlayerProgression.onLevelUp -= ElectricityLevelUp;
        instance = null;
    }
}
