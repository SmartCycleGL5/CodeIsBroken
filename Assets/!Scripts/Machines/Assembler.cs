using System;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Assembler : Machine,IItemContainer
{
    [SerializeField] int assemblerSize;
    [SerializeField] List<Item> items;
    [SerializeField] List<CraftingRecipie> craftingRecipies;

    public Item item { get; set; }

    private void Start()
    {
        Tick.OnTick += TakeItem;
        Tick.OnEndingTick += ClearMachine;
    }

    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(Assembler));

        base.Initialize(initialClassName);
    }
    private void Update()
    {

    }



    public void Craft()
    {
        if(items.Count <= 0) return;
        // Materials of items currently held by conveyor
        List<BaseMaterial> materials = new();
        materials.AddRange(items.Select(i => i.material));
        
        //Loops over all recipes
        foreach (var recipe in craftingRecipies)
        {
            // Sort lists to compare them:
            materials = materials.OrderBy(x => x.type).ToList();
            recipe.materials = recipe.materials.OrderBy(x => x.type).ToList();
            
            if (materials.SequenceEqual(recipe.materials))
            {
                ClearMachine();
                Debug.Log("Valid recipe");
                
                // output item to next conveyor
                GameObject cell = GridBuilder.instance.LookUpCell(transform.position + transform.forward);
                if(cell == null) return;
                if (cell.TryGetComponent(out Conveyor conveyor))
                {
                    conveyor.SetItem(Instantiate(recipe.itemToSpawn, transform.position, Quaternion.identity));
                    RemoveItem();
                }
            }
        }
    }

    [DontIntegrate]
    private void TakeItem()
    {
        Debug.Log("TakeItem");
        if(items.Count >= assemblerSize) return;
        GameObject cell = GridBuilder.instance.LookUpCell(transform.position - transform.forward);
        if (cell == null) return;
        if (cell.TryGetComponent(out Conveyor conveyor))
        {
            item = conveyor.item;
            if(item == null) return;
            item.transform.position = transform.position;
            items.Add(item);
            conveyor.RemoveItem();
        }
    }

    public void ClearMachine()
    {
        items.Clear();
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
        Tick.OnTick -= TakeItem;
        Tick.OnEndingTick -= ClearMachine;
    }
}
