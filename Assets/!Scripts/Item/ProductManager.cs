using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeIsBroken.Product
{
    public class ProductManager : MonoBehaviour
    {
        public static ProductManager Instance;
        [SerializedDictionary("Material", "Prefab")]
        [field: SerializeField]public SerializedDictionary<ProductDefinition, Item> Products {get; private set;}
    
        void Awake()
        {
            Instance = this;
        }
    
        public static Item GetRandomProduct(int complexity = 1)
        {
            List<KeyValuePair<ProductDefinition, Item>> listToChooseFrom = Instance.Products.ToList();

            return listToChooseFrom[Random.Range(0, listToChooseFrom.Count - 1)].Value;
        }
    }
}

