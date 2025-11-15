using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

namespace CodeIsBroken.Item
{
    [Flags]
    public enum Materials
    {
        None = 0,
        Wood = 1 << 1,
        Iron = 1 << 2,
        Stone = 1 << 3,
    }
    
    
    public class MaterialManager : MonoBehaviour
    {
        public static MaterialManager Instance;
        [SerializedDictionary("Material", "Prefab")]
        public SerializedDictionary<Materials, Item> Products;
    
    
        void Awake()
        {
            Instance = this;
        }
    
        public static Materials GetRandomProduct(int complexity = 1)
        {
            Materials materials;
            
            while(true)
            {
                materials = Materials.None;
    
                for (int i = 0; i < complexity; i++)
                {
                    materials |= RandomMaterial();
                }
    
                if (Instance.Products.ContainsKey(materials))
                {
                    break;
                }
            }
    
            return materials;
        }
    
        public static Materials RandomMaterial()
        {
            Array materials = Enum.GetValues(typeof(Materials));
    
            int rng = UnityEngine.Random.Range(1, materials.Length);
    
            return (Materials)materials.GetValue(rng);
        }
    }
}

