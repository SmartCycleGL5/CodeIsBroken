using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

public class Assembler : MonoBehaviour, IItemContainer
{
    [SerializeField] int assemblerSize;
    [SerializeField] List<Item> items;
    [SerializeField] List<CraftingRecipie> craftingRecipies;

    public Item item { get; set; }

    public void Clear()
    {
        items.Clear();
    }
    private void Update()
    {

    }



    public void Craft()
    {

    }

    public bool RemoveItem(out Item removedItem)
    {
        removedItem = null;
        if (item == null) return false;
        removedItem = item;
        item = null;

        return true;
    }

    public bool SetItem(Item item)
    {
        if (this.item != null) return false;
        this.item = item;
        this.item.transform.position = transform.position;
        return true;
    }
    [DontIntegrate]
    public bool RemoveItem()
    {
        return RemoveItem(out Item item);
    }

    private void OnDestroy()
    {
        
    }
}
