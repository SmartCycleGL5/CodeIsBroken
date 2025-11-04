using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class Metrics : MonoBehaviour
{
    public static Metrics instance;
    
    private int totalElectricity;
    private int timeSinceLevelUp;

    [SerializeField] List<int> electricityLevel = new();

    [SerializeField]List<int> timeUsed = new();

    private void Start()
    {
        instance = this;
        PlayerProgression.onLevelUp += TimeLevelUp;
        PlayerProgression.onLevelUp += ElectricityLevelUp;
        PlayerProgression.onLevelUp += GenerateGraph;
    }

    // Used for machines to add electricity,
    public void UseElectricity(int increase)
    {
        totalElectricity += increase;
    }

    // Adds electricity to metrics on level up
    private void ElectricityLevelUp(int level)
    {
        electricityLevel.Add(totalElectricity);
        totalElectricity = 0;
    }

    // Adds time to metrics on level up
    private void TimeLevelUp(int level)
    {
        int time = (int)Time.time;
        timeUsed.Add(time - timeSinceLevelUp);
        timeSinceLevelUp = time;
    }

    
    

    public void GenerateGraph(int level)
    {
        if(level <=3) return;
        GraphDrawer.instance.DrawCharts(electricityLevel, timeUsed);
    }

    private void OnDestroy()
    {
        PlayerProgression.onLevelUp -= TimeLevelUp;
        PlayerProgression.onLevelUp -= ElectricityLevelUp;
        instance = null;
    }
}
