using System;
using System.Collections.Generic;
using CodeIsBroken.Product;
using CodeIsBroken.Product.Modifications;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

namespace CodeIsBroken.Contract
{
   public class Contract
    {
        public string contractName;
        [HideInInspector] public ProductDefinition RequestedProduct;
        public int amount;
        public int amountLeft { get; private set; }
    
        public Action<Contract> onFinished;
        public Action<int,int> onProgress;
        public int xpToGive =>
            Mathf.RoundToInt(((RequestedProduct.mods.Count + 1) * 5) * (amount / 2));
        private static int amountOfMods => Random.Range(
            ContractGiver.activeSettings.additionalModifications.x,
            ContractGiver.activeSettings.additionalModifications.y);
        
        public static Contract New()
        {
            Contract contract = new Contract();
            contract.RequestedProduct = ProductManager.GetRandomProduct().definition.Clone();
            contract.amount = Mathf.RoundToInt(UnityEngine.Random.Range(PlayerProgression.Level * 5, (PlayerProgression.Level * 5) * 2));
            contract.amountLeft = contract.amount;
            
    
            IModification[] additionalModifications = GetRandomModifications(amountOfMods); 
    
            foreach (var mod in additionalModifications)
            {
                contract.RequestedProduct.Modify(mod);
            }

            return contract;
        }
        public static Contract New(Item toRequest, int amount, IModification[] additionalModifications = null)
        {
            Contract contract = new Contract();
            contract.RequestedProduct = toRequest.definition.Clone();
            contract.amount = amount;
            contract.amountLeft = contract.amount;
            
            
            foreach (var mod in additionalModifications)
            {
                contract.RequestedProduct.Modify(mod);
            }

            return contract;
        }
        public void Progress()
        {
            Debug.Log("[Contract] Progress");
            amountLeft--;
            onProgress?.Invoke(amountLeft,amount);
            if (amountLeft <= 0)
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
        
        static IModification[] GetRandomModifications(int amount)
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
}
