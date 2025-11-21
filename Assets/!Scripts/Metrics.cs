using System.Collections.Generic;
using UnityEngine;

public class Metrics : MonoBehaviour
{
    public static Metrics instance;

    private int totalElectricity;
    private int timeSinceLevelUp;

    [SerializeField] List<int> electricityLevel = new();

    [SerializeField] List<int> timeUsed = new();

    private bool sentData;

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
        if (level == 5)
        {
            sentData = true;
            //GraphDrawer.instance.DrawCharts(electricityLevel, timeUsed);
            SendData(false);
        }

    }

    private void OnApplicationQuit()
    {
        if (sentData) return;
        SendData(true);
    }

    private void SendData(bool quit)
    {
        string electricityString = string.Join(", ", electricityLevel.ToArray());
        string timeString = string.Join(", ", timeUsed.ToArray());

        try
        {
            Webhook.instance.SendStats(electricityString, timeString, quit);
        }
        catch
        {
            Debug.Log("[Metrics] Webhook disabled");
        }
    }


    private void OnDestroy()
    {
        PlayerProgression.onLevelUp -= TimeLevelUp;
        PlayerProgression.onLevelUp -= ElectricityLevelUp;
        instance = null;
    }

}

