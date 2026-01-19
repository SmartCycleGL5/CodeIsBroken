using System.Collections.Generic;
using CodeIsBroken.ProductSystem;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRecipie item")]
public class CraftingRecipie : ScriptableObject
{

    [Header("Materials")] public List<Product> materials;
    public Product itemToSpawn;

    [Header("Settings")]
    public int tickCraftingTime;

}
