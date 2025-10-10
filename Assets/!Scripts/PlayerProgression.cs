using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class PlayerProgression
{
    public static Dictionary<int, int> experienceRequired = new()
    {
        { 1, 10 },
        { 2, 100 },
        { 3, 300 },
        { 4, 500 },
    };

    public static int Level { get; private set; } = 1;
    public static int experience { get; private set; }
    public static float apparentExperience { get; private set; }

    public static Action<int> onLevelUp;

    public static void LevelUp()
    {
        Level++;
        experience = 0;
        apparentExperience = 0;
        onLevelUp?.Invoke(Level);
    }

    static bool gettingXP;
    
    public static async void GiveXP(int amount)
    {
        experience += amount;

        if (gettingXP) return;

        while(apparentExperience < experience)
        {
            gettingXP = true;
            apparentExperience += Time.deltaTime * 5;
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }
        gettingXP = false;

        apparentExperience = experience;

        if (experience >= experienceRequired[Level])
        {
            LevelUp();
        }
    }
}
