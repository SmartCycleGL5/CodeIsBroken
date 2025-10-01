using AYellowpaper.SerializedCollections;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    public static MaterialManager Instance;
    [SerializedDictionary("Name", "Prefab")]
    public SerializedDictionary<Materials, Item> items;

    void Awake()
    {
        Instance = this;
    }
}
