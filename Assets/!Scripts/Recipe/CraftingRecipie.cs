using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewRecipie item")]
public class CraftingRecipie : ScriptableObject
{
    [Header("Materials")]
    public List<Item> itemsRequired;
    public Item craftedMaterial;

    [Header("Settings")]
    public int tickCraftingTime;

}
