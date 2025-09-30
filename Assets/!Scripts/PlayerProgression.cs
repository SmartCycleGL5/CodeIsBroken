using System;
using UnityEngine;

public static class PlayerProgression
{
    public static int Level {  get; private set; }
    public static int experience { get; private set; }

    public static Action<int> onLevelUp;

    public static void LevelUp()
    {
        Level++;
        onLevelUp?.Invoke(Level);
    }
    
    public static void GiveXP(int amount)
    {
        experience += amount;
    }
}
