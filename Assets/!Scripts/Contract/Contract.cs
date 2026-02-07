using CodeIsBroken.ProductSystem;
using CodeIsBroken.ProductSystem.Modifications;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace CodeIsBroken.Contract
{
    public class Contract
    {
        public class Request
        {
            public Product product;
            public IModification[] modifications;
            public int amount;
            public int amountLeft { get; private set; }

            public bool satisfied { get; private set; }
            public Action onSatisfied;
            public int xp => Mathf.RoundToInt(((product.modifications.Count + 1) * 5) * (amount / 2));

            public Request(Product product, int amount, IModification[] modifications = null)
            {
                this.product = product;
                this.modifications = modifications;
                this.amount = amount;
                this.amountLeft = amount;
            }
            public void Progress()
            {
                amountLeft--;
                if (amountLeft <= 0)
                {
                    satisfied = true;
                    onSatisfied?.Invoke();
                }
            }
            public bool SatisfiesRequest(Product product)
            {
                if(satisfied) return false;
                if(!this.product.Equals(product)) return false;
                if(modifications.Length != product.modifications.Count) return false;

                for (int i = 0; i < modifications.Length; i++)
                {
                    if(!modifications[i].Equals(product.modifications[i])) return false;
                }
                
                return true;
            }

            public TemplateContainer GetUI()
            {
                TemplateContainer request = ContractManager.requestUI.Instantiate();

                request.Q<Label>("Amount").text = amount.ToString() + " X";
                request.Q<Label>("MaterialTitle").text = product.pruductType.ToString();
                request.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(product.icon);

                ScrollView mods = request.Q<ScrollView>("ModView");

                foreach (var mod in modifications)
                {
                    TemplateContainer modifierContainer = ContractManager.modifierUI.Instantiate();

                    VisualElement modifier = modifierContainer.Q<VisualElement>("Modifier");
                    modifier.Q<Label>("Name").text = mod.Name;
                    modifier.Q<Label>("Description").text = "";//mod.Description;

                    mods.Add(modifier);
                }

                return request;
            }
        }

        public string companyName;

        public Request[] requests;

        public Action<Contract> onFinished;
        public Action onProgress;

        public int xpToGive
        {
            get
            {
                int total = 0;

                foreach (var item in requests)
                {
                    total = item.xp;
                }
                return total;
            }
        }
        private static int amountOfMods => Random.Range(
            ContractManager.activeSettings.additionalModifications.x,
            ContractManager.activeSettings.additionalModifications.y);

        public Contract(Request[] requests)
        {
            companyName = ContractManager.GetCompanyName();
            this.requests = requests;
        }

        /// <summary>
        /// Creates a random new contract
        /// </summary>
        /// <returns>the contract</returns>
        public static Contract New()
        {
            Product RequestedProduct = ProductManager.GetRandomProduct();
            int amount = Mathf.RoundToInt(Random.Range(PlayerProgression.Level * 5, (PlayerProgression.Level * 5) * 2));


            Request request = new Request(RequestedProduct, amount);
            return New(new Request[] { request });
        }
        /// <summary>
        /// Creates a predetermined contract
        /// </summary>
        /// <param name="requests">the requests made by the contract</param>
        /// <returns>the contract</returns>
        public static Contract New(Request[] requests)
        {
            Contract contract = new Contract(requests);

            return contract;
        }
        public void Finish()
        {
            PlayerProgression.LevelUp();
            //PlayerProgression.GiveXP(xpToGive);
            onFinished?.Invoke(this);
        }

        public bool TryProgressContract(Product product)
        {
            foreach (var request in requests)
            {
                if(request.SatisfiesRequest(product))
                {
                    request.Progress();
                    onProgress?.Invoke();

                    if(allRequestsSatisfied())
                    {
                        Finish();
                    }
                    
                    return true;
                }
            }
            
            return false;
        }

        bool allRequestsSatisfied()
        {
            foreach (var request in requests)
            {
                if (!request.satisfied) return false;
            }

            return true;
        }

        public TemplateContainer GetUI()
        {
            TemplateContainer contract = ContractManager.contractUI.Instantiate();

            contract.Q<Label>("ContractName").text = companyName;

            foreach (var request in requests)
            {

                Tab tab = new Tab(request.product.name);
                tab.Add(request.GetUI());
                contract.Q<TabView>("Requests").Add(tab);
            }

            return contract;
        }
    }
}
