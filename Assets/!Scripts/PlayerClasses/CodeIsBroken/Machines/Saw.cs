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
    public class Saw : Machine, IItemContainer
    {
        private int sawSize = 1;
        private List<Item> items = new();
        private List<CraftingRecipie> craftingRecipies;

        private Transform inputPos;
        private Transform outputPos;
        private Transform sawBlade;
        
        private ParticleSystem sawParticles;
        
        private ReferenceHolder referenceHolder;
        Tweener moveTween;
        public Item item { get; set; }
    
    
        void Start()
        {
            craftingRecipies = GetComponent<RecipeHolder>().GetRecipes();
            
            Programmable machine = GetComponent<Programmable>();
            machine.AddMethodsAsIntegrated(typeof(Saw));

            referenceHolder = GetComponent<ReferenceHolder>();
            inputPos = referenceHolder.GetReference("input").transform;
            outputPos = referenceHolder.GetReference("output").transform;
            sawBlade = referenceHolder.GetReference("sawBlade").transform;
            sawParticles = referenceHolder.GetReference("sawParticle").GetComponent<ParticleSystem>();
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
    
        public void Cut()
        {
            //Start sawblade animation
            sawBlade.DOLocalRotate(new Vector3(560,0,0), 0.3f, RotateMode.FastBeyond360);
            
            Metrics.instance.UseElectricity(1);
            Item itemCrafted = Crafting.instance.CraftItem(items, craftingRecipies);
            
            if (itemCrafted != null)
            {
                Debug.Log("Valid recipe");
                
                // output item to next conveyor
                GameObject cell = GridBuilder.instance.LookUpCell(outputPos.position + outputPos.forward);
                if(cell == null) return;
                if (cell.TryGetComponent(out Conveyor conveyor))
                {
                    if (conveyor.item != null) return;
                    sawParticles.Play();
                    Destroy(item.gameObject);
                    item = Instantiate(itemCrafted, sawBlade.position, Quaternion.identity);
                    conveyor.SetItem(item);
                    ClearMachine();
                    RemoveItem();

                    
                }
            }
            else
            {
                if (items.Count == sawSize)
                {
                    PlayerConsole.LogWarning($"Cant cut {item.definition.baseMaterials}");
                    // Cant craft using this item, send error
                }
            }
        }
        
        
        private void TakeItem()
        {
            Debug.Log("[Saw] TakeItem");
            if(items.Count >= sawSize) return;
            GameObject cell = GridBuilder.instance.LookUpCell(inputPos.position - inputPos.forward);
            if (cell == null) return;
            if (cell.TryGetComponent(out Conveyor conveyor))
            {
                item = conveyor.item;
                if(item == null) return;
                items.Add(item);
                moveTween = item.gameObject.transform.DOMove(sawBlade.position,0.2f);
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
            moveTween = this.item.gameObject.transform.DOMove(inputPos.position + new Vector3(0,2,0),0.2f);
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


