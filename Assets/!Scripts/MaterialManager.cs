using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

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
    public SerializedDictionary<Materials, Item> BaseMaterials;

    [SerializedDictionary("Material", "Prefab")]
    public SerializedDictionary<Materials, Item> Products;

    void Awake()
    {
        Instance = this;
    }

    public static Materials GetRandomMaterial(int amount = 1)
    {
        Materials materials = Materials.None;

        for (int i = 0; i < amount; i++)
        {
            materials |= RandomMaterial();
        }

        return materials;

        Materials RandomMaterial()
        {
            Array materials = Enum.GetValues(typeof(Materials));

            int rng = UnityEngine.Random.Range(1, materials.Length);

            return (Materials)materials.GetValue(rng);
        }
    }
}
