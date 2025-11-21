using System;
using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Linq;
using CodeIsBroken.Product;
using DG.Tweening;
using ScriptEditor.Console;
using UnityEngine;

namespace CodeIsBroken
{
    public class Furnace : Machine, IItemContainer
    {
        private int furnaceSize = 1;
        private List<Item> items = new();
        private List<CraftingRecipie> craftingRecipies;
        
        private ParticleSystem furnaceParticle;
        
        private ReferenceHolder referenceHolder;
        Tweener moveTween;
        public Item item { get; set; }
        Programmable programmable;


        void Start()
        {
            craftingRecipies = GetComponent<RecipeHolder>().GetRecipes();
            
            Programmable machine = GetComponent<Programmable>();
            machine.AddMethodsAsIntegrated(typeof(Furnace));

            referenceHolder = GetComponent<ReferenceHolder>();
            
            Tick.OnTick += TakeItem;
        }
    
        private void Reset()
        {
            ClearMachine();
        }

        public int ItemCount()
        {
            return items.Count;
        }
    
        public void Heat()
        {
            //
            
            Metrics.instance.UseElectricity(1);
            Item itemCrafted = Crafting.instance.CraftItem(items, craftingRecipies);
            
            if (itemCrafted != null)
            {
                Debug.Log("Valid recipe");
                
                // output item to next conveyor
                GameObject cell = GridBuilder.instance.LookUpCell(transform.position+transform.forward);
                if(cell == null) return;
                if (cell.TryGetComponent(out Conveyor conveyor))
                {
                    if (conveyor.item != null) return;
                    // sawParticles.Play();
                    Destroy(item.gameObject);
                    item = Instantiate(itemCrafted, transform.position, Quaternion.identity);
                    conveyor.SetItem(item);
                    ClearMachine();
                    RemoveItem();
                }
            }
            else
            {
                if (items.Count == furnaceSize)
                {
                    PlayerConsole.LogWarning($"Cant heat {item.definition.baseMaterials}", programmable.attachedScripts[0].name);
                    // Cant craft using this item, send error
                }
            }
        }
        
        
        private void TakeItem()
        {
            Debug.Log("[Furnace] TakeItem");
            if(items.Count >= furnaceSize) return;
            GameObject cell = GridBuilder.instance.LookUpCell(transform.position - transform.forward);
            if (cell == null) return;
            if (cell.TryGetComponent(out Conveyor conveyor))
            {
                item = conveyor.item;
                if(item == null) return;
                items.Add(item);
                moveTween = item.gameObject.transform.DOMove(transform.position,0.2f);
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
            if (moveTween != null)
            {
                moveTween.Kill();
            }
            return true;
        }

        public bool SetItem(Item item)
        {
            if (this.item != null) return false;
            this.item = item;
            moveTween = this.item.gameObject.transform.DOMove(transform.position + new Vector3(0,2,0),0.2f);
            return true;
        }

        public bool RemoveItem()
        {
            return RemoveItem(out Item item);
        }
    

        protected void OnDestroy()
        {
            Tick.OnTick -= TakeItem;
        }
    }
}


