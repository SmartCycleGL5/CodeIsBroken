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
        public Item toRequest;
        public int amount;

        [field: SerializeReference, SubclassSelector]
        public IAdditionalModification[] additionalModifications;

        public Contract GetContract()
        {
            return Contract.New(toRequest, amount, additionalModifications);
        }
    }
}
