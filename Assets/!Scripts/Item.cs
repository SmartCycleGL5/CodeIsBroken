using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public BaseMaterial material;
    [field: SerializeField] public List<Modification> mods { get; private set; } = new List<Modification>();

    public MeshRenderer artRenderer;
    public static List<Item> items = new List<Item>();

    private void Start()
    {
        artRenderer.material.SetFloat("_Rng", UnityEngine.Random.Range(10, 100));
        items.Add(this);
    }
    private void OnDestroy()
    {
        items.Remove(this);
    }

    public void Modify(Modification modification)
    {
        mods.Add(modification);
    }

    public bool HasMod<T>(T toCompareWith) where T : Modification
    {
        foreach (var mod in mods)
        {
            if (mod is T)
            {
                return mod.Compare(toCompareWith);
            }
        }
        return false;
    }
}
[Serializable]
public struct BaseMaterial
{
    public enum Materials
    {
        Wood,
        Steel,
    }

    public Materials type;

    public BaseMaterial(Materials type)
    {
        this.type = type;
    }
}
