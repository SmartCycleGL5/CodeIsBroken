using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using NaughtyAttributes;
using Random = UnityEngine.Random;

namespace CodeIsBroken.ProductSystem
{
    public class ProductManager : MonoBehaviour
    {
        public static ProductManager Instance;



        [SerializeField, SerializedDictionary("Product", "Prefab"), ReadOnly]
        private SerializedDictionary<CodeIsBroken.Product, Product> Products;
        public static Action foundProducts;
        
        void Awake()
        {
            Instance = this;
        }

        private async void Start()
        {
            List<GameObject> items = await Addressable.LoadAssets<GameObject>("Product");

            foreach (var item in items)
            {
                Product i = item.GetComponent<Product>();
                Products.Add(i.pruductType, i);
            }
            
            foundProducts?.Invoke();
        }

        public static Product GetRandomProduct()
        {
            List<KeyValuePair<CodeIsBroken.Product, Product>> listToChooseFrom = Instance.Products.ToList();

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

        public static Product GetProduct(CodeIsBroken.Product toFind)
        {
            return Instance.Products[toFind];
        }
    }
}

