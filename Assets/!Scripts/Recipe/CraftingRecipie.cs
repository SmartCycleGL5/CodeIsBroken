using System.Collections.Generic;
using CodeIsBroken.Item;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRecipie item")]
public class CraftingRecipie : ScriptableObject
{

    [Header("Materials")] public List<Materials> materials;
    public Item itemToSpawn;

    [Header("Settings")]
    public int tickCraftingTime;

}
