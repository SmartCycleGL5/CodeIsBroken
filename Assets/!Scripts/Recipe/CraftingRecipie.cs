using System.Collections.Generic;
using CodeIsBroken.Product;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRecipie item")]
public class CraftingRecipie : ScriptableObject
{

    [Header("Materials")] public List<BaseMaterials> materials;
    public Item itemToSpawn;

    [Header("Settings")]
    public int tickCraftingTime;

}
