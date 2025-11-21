using CodeIsBroken.Product;
using CodeIsBroken.Product.Modifications;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace CodeIsBroken.Contract
{
    public class Contract
    {
        public class Request
        {
            public ProductDefinition product;
            public int amount;
            public int amountLeft { get; private set; }

            public bool satisfied { get; private set; }
            public Action onSatisfied;

            public int xp => Mathf.RoundToInt(((product.mods.Count + 1) * 5) * (amount / 2));

            public Request(ProductDefinition product, int amount, IAdditionalModification[] additionalModifications = null)
            {
                this.product = product.Clone();
                this.amount = amount;
                this.amountLeft = amount;

                foreach (var mod in additionalModifications)
                {
                    this.product.Modify(mod);
                }
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
            public bool SatisfiesRequest(ProductDefinition product)
            {
                if(satisfied) return false;
                return product.Equals(product);
            }

            public TemplateContainer GetUI()
            {
                TemplateContainer request = ContractManager.requestUI.Instantiate();

                request.Q<Label>("Amount").text = amount.ToString() + " X";
                request.Q<Label>("MaterialTitle").text = product.baseMaterials.ToString();
                request.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(product.icon);

                ScrollView mods = request.Q<ScrollView>("ModView");

                foreach (var mod in product.mods)
                {
                    TemplateContainer modifierContainer = ContractManager.modifierUI.Instantiate();

                    VisualElement modifier = modifierContainer.Q<VisualElement>("Modifier");
                    modifier.Q<Label>("Name").text = mod.Name;
                    modifier.Q<Label>("Description").text = mod.Description;

                    mods.Add(modifier);
                }

                return request;
            }
        }

        public string companyName;

        public Request[] requests;

        public Action<Contract> onFinished;

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
            ProductDefinition RequestedProduct = ProductManager.GetRandomProduct().definition.Clone();
            int amount = Mathf.RoundToInt(Random.Range(PlayerProgression.Level * 5, (PlayerProgression.Level * 5) * 2));


            Request request = new Request(RequestedProduct, amount, IAdditionalModification.GetRandomModifications(amountOfMods));
            return Contract.New(new Request[] { request });
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
            PlayerProgression.GiveXP(xpToGive);
            onFinished?.Invoke(this);
        }

        public bool TryProgressContract(Item item)
        {
            foreach (var request in requests)
            {
                if(request.SatisfiesRequest(item.definition))
                {
                    request.Progress();

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
