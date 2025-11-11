using System;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Assembler : Machine, IItemContainer
{
    [SerializeField] int assemblerSize;
    [SerializeField] List<Item> items;
    [SerializeField] List<CraftingRecipie> craftingRecipies;
    private UserErrorLogger errorLogger;
    
    Tweener moveTween;
    public Item item { get; set; }


    //protected override void Start() no more start
    //{
    //    AddMethodsAsIntegrated(typeof(Assembler));
    //    base.Start();
    //    Tick.OnTick += TakeItem;
    //    errorLogger = GetComponent<UserErrorLogger>();
    //}

    public override void Reset()
    {
        ClearMachine();
    }

    public void Craft()
    {
        Metrics.instance.UseElectricity(1);
        Item itemCrafted = Crafting.instance.CraftItem(items, craftingRecipies);
        if (itemCrafted != null)
        {
            ClearMachine();
            Debug.Log("Valid recipe");
            
            // output item to next conveyor
            GameObject cell = GridBuilder.instance.LookUpCell(transform.position + transform.forward);
            if(cell == null) return;
            if (cell.TryGetComponent(out Conveyor conveyor))
            {
                Item item = Instantiate(itemCrafted, transform.position, Quaternion.identity);
                //item.definition.Modify(new Modification.Assemble(recipe.name));

                conveyor.SetItem(item);
                RemoveItem();
            }
        }
        else
        {
            if (items.Count == assemblerSize)
            {
                errorLogger.DisplayWarning("Materials does not match any recipe's");
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

    [DontIntegrate]
    public bool RemoveItem(out Item removedItem)
    {
        removedItem = null;
        if (item == null) return false;
        removedItem = item;
        item = null;
        if (moveTween != null)
        {
            moveTween.Kill();
        }

        return true;
    }
    [DontIntegrate]
    public bool SetItem(Item item)
    {
        if (this.item != null) return false;
        this.item = item;
        moveTween = this.item.gameObject.transform.DOMove(transform.position+new Vector3(0,1,0),0.3f);
        return true;
    }
    [DontIntegrate]
    public bool RemoveItem()
    {
        return RemoveItem(out Item item);
    }

    [DontIntegrate]
    protected void OnDestroy()
    {
        Tick.OnTick -= TakeItem;
    }
}
