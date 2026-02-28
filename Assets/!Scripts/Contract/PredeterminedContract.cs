using System;
using System.Collections.Generic;
using CodeIsBroken.ProductSystem;
using CodeIsBroken.ProductSystem.Modifications;
using UnityEngine;
using UnityEngine.Serialization;

namespace CodeIsBroken.Contract
{
    [Serializable]
    public struct PredeterminedContract
    {
        [Serializable]
        public struct Request
        {
            public Product toRequest;
            public int amount;
            [FormerlySerializedAs("additionalModifications")] [field: SerializeReference, SubclassSelector]
            public IModification[] mods;

            public Contract.Request GetRequest()
            {
                return new Contract.Request(ProductManager.GetProduct(toRequest), amount, mods);
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
