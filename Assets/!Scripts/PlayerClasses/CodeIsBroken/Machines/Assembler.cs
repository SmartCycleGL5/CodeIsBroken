using System;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using CodeIsBroken.Item;
using DG.Tweening;
using UnityEngine;

namespace CodeIsBroken
{
    public class Assembler : Machine, IItemContainer
    {
        [SerializeField] int assemblerSize;
        [SerializeField] List<Item.Item> items;
        [SerializeField] List<CraftingRecipie> craftingRecipies;
        private UserErrorLogger errorLogger;
        
        Tweener moveTween;
        public Item.Item item { get; set; }
    
    
        void Start()
        {
            Programmable machine = GetComponent<Programmable>();
            machine.AddMethodsAsIntegrated(typeof(Assembler));

            Tick.OnTick += TakeItem;
            errorLogger = GetComponent<UserErrorLogger>();
        }
    
        private void Reset()
        {
            ClearMachine();
        }
        
        public int ItemCount()
        {
            return items.Count;
        }
    
        public void Craft()
        {
            Metrics.instance.UseElectricity(1);
            Item.Item itemCrafted = Crafting.instance.CraftItem(items, craftingRecipies);
            if (itemCrafted != null)
            {
                ClearMachine();
                Debug.Log("Valid recipe");
                
                // output item to next conveyor
                GameObject cell = GridBuilder.instance.LookUpCell(transform.position + transform.forward);
                if(cell == null) return;
                if (cell.TryGetComponent(out Conveyor conveyor))
                {
                    Item.Item item = Instantiate(itemCrafted, transform.position, Quaternion.identity);
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
    

        public bool RemoveItem(out Item.Item removedItem)
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

        public bool SetItem(Item.Item item)
        {
            if (this.item != null) return false;
            this.item = item;
            moveTween = this.item.gameObject.transform.DOMove(transform.position+new Vector3(0,1,0),0.3f);
            return true;
        }

        public bool RemoveItem()
        {
            return RemoveItem(out Item.Item item);
        }
    

        protected void OnDestroy()
        {
            Tick.OnTick -= TakeItem;
        }
    }
}


