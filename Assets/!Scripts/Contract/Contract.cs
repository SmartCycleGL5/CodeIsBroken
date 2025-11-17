using System;
using System.Collections.Generic;
using CodeIsBroken.Product;
using CodeIsBroken.Product.Modifications;
using UnityEngine;

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
    
        public Contract(string name, int amountOfMods, int complexity)
        {
            contractName = names[UnityEngine.Random.Range(0, names.Length - 1)];
            ProductDefinition product = ProductManager.GetRandomProduct().definition;
            RequestedProduct = product;
    
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
    
            foreach (var additionalMod in mods)
            {
                RequestedProduct.Modify(additionalMod);
            }
            
            amount = Mathf.RoundToInt(UnityEngine.Random.Range(PlayerProgression.Level * 5, (PlayerProgression.Level * 5) * 2));
    
            float xp = ((RequestedProduct.mods.Count + 1) * 3 + complexity * 3) * 1.5f;
            xpToGive = Mathf.RoundToInt(xp * (amount / 3));
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
