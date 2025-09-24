using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

public class Assembler : MonoBehaviour
{
    [SerializeField] int assemblerSize;
    [SerializeField] List<Item> testingItems;
    [SerializeField] HashSet<Item> items;
    [SerializeField] List<CraftingRecipie> craftingRecipies;

    public void Clear()
    {
        items.Clear();
    }
    private void Update()
    {
        Craft();
    }
    public void Craft()
    {
        if(craftingRecipies.Count == 0 || testingItems.Count == 0) return;
        items = new HashSet<Item>(testingItems);
        foreach (var recipie in craftingRecipies)
        {
            if(recipie.itemsRequired.SetEquals(items))
            {
                Debug.Log("Matching");
            }
        }
    }

}
