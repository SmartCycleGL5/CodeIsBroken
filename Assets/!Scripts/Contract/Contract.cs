using System;
using System.Collections.Generic;
using CodeIsBroken.Product;
using CodeIsBroken.Product.Modifications;
using UnityEngine;
using Color = UnityEngine.Color;

namespace CodeIsBroken.Contract
{
   public class Contract
    {
        public string contractName;
        public ProductDefinition RequestedProduct;
        public int amount;
    
        public Action<Contract> onFinished;
    
        public int xpToGive;
    
        readonly string[] names = new string[]
        {
            "Wood",
            "Stone",
            "Iron",
            "Wood Carvers",
            "Stone Carvers"
        };
    
        public Contract(string name, int amountOfMods)
        {
            contractName = names[UnityEngine.Random.Range(0, names.Length - 1)];

            RequestedProduct = ProductManager.GetRandomProduct().definition.Clone();
    
            IModification[] additionalModifications = GetRandomModifications(amountOfMods); 
    
            foreach (var mod in additionalModifications)
            {
                RequestedProduct.Modify(mod);
            }
            
            amount = Mathf.RoundToInt(UnityEngine.Random.Range(PlayerProgression.Level * 5, (PlayerProgression.Level * 5) * 2));
    
            float xp = ((RequestedProduct.mods.Count + 1) * 3 + RequestedProduct.mods.Count * 3) * 1.5f;
            xpToGive = Mathf.RoundToInt(xp * (amount / 2));
            
            IModification[] GetRandomModifications(int amount)
            {
                List<IModification> mods = new List<IModification>();
                for (int i = 0; i < amountOfMods; i++)
                {
                    IModification newMod = IModification.RandomModification();
    
                    if (mods.Contains(newMod))
                    {
                        Debug.Log("already has mod");
                        continue;
                    }
    
                    Debug.Log(newMod);
                    mods.Add(newMod);
                }

                return mods.ToArray();
            }
        }
        public void Progress()
        {
            Debug.Log("[Contract] Progress");
            amount--;
    
            if (amount <= 0)
            {
                Finish();
            }
        }
        public void Finish()
        {
            PlayerProgression.GiveXP(xpToGive);
            onFinished?.Invoke(this);
        }
    
        public bool SatisfiesContract(Item item)
        {
            return item.definition.Equals(RequestedProduct);
        }
    }
}
