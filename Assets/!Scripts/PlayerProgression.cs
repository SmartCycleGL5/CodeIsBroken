using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class PlayerProgression
{
    public static Dictionary<int, int> experienceRequired = new()
    {
        { 1, 10 },
        { 2, 30 },
        { 3, 60 },
        { 4, 100 },
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
    
    public static async void GiveXP(int amount)
    {
        apparentExperience = experience;
        experience += amount;

        Debug.Log(apparentExperience < experience);

        while(apparentExperience < experience)
        {
            apparentExperience += Time.deltaTime * 5;
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        apparentExperience = experience;

        Debug.Log(experience + " - " + experienceRequired[Level]);

        if (experience >= experienceRequired[Level])
        {
            LevelUp();
        }
    }
}
