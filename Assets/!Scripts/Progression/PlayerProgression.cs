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
        { 5, 700 },
        { 6, 1000 },
        { 7, 1500 },
        { 8, 2000 },
        { 9, 3000 },
        { 10, 5000 },
    };

    public static int Level { get; private set; } = 1;
    public static int experience { get; private set; }
    public static float apparentExperience { get; private set; }

    public static Action<int> onLevelUp;

    public static void LevelUp()
    {
        experience -= experienceRequired[Level];
        apparentExperience = 0;

        Level++;

        onLevelUp?.Invoke(Level);
    }

    static bool gettingXP;
    
    public static void GiveXP(int amount)
    {
        Debug.Log("GOT XP " + amount);
        experience += amount;
        apparentExperience = experience;

        if (experience >= experienceRequired[Level])
        {
            LevelUp();

            //if (gettingXP) return;
            //UpdateXP(Level -1);

            return;
        }

        //if (gettingXP) return;
        //UpdateXP(Level);
    }

    static async void UpdateXP(int level)
    {

        while (apparentExperience < experienceRequired[level])
        {
            gettingXP = true;
            apparentExperience += Time.deltaTime * 5;
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }
        gettingXP = false;

        apparentExperience = experience;
    }
}
