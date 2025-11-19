using System;
using System.Collections.Generic;
using CodeIsBroken.Product;
using CodeIsBroken.Product.Modifications;
using UnityEngine;

namespace CodeIsBroken.Contract
{
    [Serializable]
    public struct PredeterminedContract
    {
        [Serializable]
        public struct Request
        {
            public Item toRequest;
            public int amount;
            [field: SerializeReference, SubclassSelector]
            public IAdditionalModification[] additionalModifications;

            public Contract.Request GetRequest()
            {
                return new Contract.Request(toRequest.definition, amount, additionalModifications);
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
