using System;
using System.Collections.Generic;
using CodeIsBroken.ProductSystem;
using CodeIsBroken.ProductSystem.Modifications;
using UnityEngine;

namespace CodeIsBroken.Contract
{
    [Serializable]
    public struct PredeterminedContract
    {
        [Serializable]
        public struct Request
        {
            public Products toRequest;
            public int amount;
            [field: SerializeReference, SubclassSelector]
            public IAdditionalModification[] additionalModifications;

            public Contract.Request GetRequest()
            {
                return new Contract.Request(ProductManager.GetProduct(toRequest), amount, additionalModifications);
            }
        }

        public Request[] requests;

        public Contract GetContract()
        {
            List<Contract.Request> officialRequests= new List<Contract.Request>();

            foreach (var item in requests)
            {
                officialRequests.Add(item.GetRequest());
            }

            return Contract.New(officialRequests.ToArray());
        }
    }
}
