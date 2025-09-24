using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRecipie item")]
public class CraftingRecipie : ScriptableObject
{
    [Header("Materials")]
    public HashSet<Item> itemsRequired;
    public Item craftedMaterial;

    [Header("Settings")]
    public int tickCraftingTime;

}
