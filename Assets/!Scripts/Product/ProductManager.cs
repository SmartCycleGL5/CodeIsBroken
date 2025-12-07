using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using NaughtyAttributes;
using Random = UnityEngine.Random;

namespace CodeIsBroken.Product
{
    public class ProductManager : MonoBehaviour
    {
        public static ProductManager Instance;

        [SerializedDictionary("Material", "Prefab"), SerializeField, ReadOnly]
        private SerializedDictionary<ProductDefinition, Item> Products;
        public static Action foundProducts;
        
        void Awake()
        {
            Instance = this;
        }

        private async void Start()
        {
            List<GameObject> items = await Addressable.LoadAssets<GameObject>("Material");

            foreach (var item in items)
            {
                Item i = item.GetComponent<Item>();
                Products.Add(i.definition, i);
            }
            
            foundProducts?.Invoke();
        }

        public static Item GetRandomProduct()
        {
            List<KeyValuePair<ProductDefinition, Item>> listToChooseFrom = Instance.Products.ToList();

            for (int i = listToChooseFrom.Count - 1; i >= 0; i--)
            {
                if (PlayerProgression.Level < listToChooseFrom[i].Value.lvlUnlock)
                {
                    Debug.Log("Removed: "  + listToChooseFrom[i].Value.name);
                    listToChooseFrom.Remove(listToChooseFrom[i]);   
                }
            }

            return listToChooseFrom[Random.Range(0, listToChooseFrom.Count)].Value;
        }

        public static Item GetProduct(ProductDefinition toFind)
        {
            foreach (var product in Instance.Products)
            {
                if (toFind.Equals(product.Key, false))
                    return product.Value;
            }
            
            return null;
        }
    }
}

