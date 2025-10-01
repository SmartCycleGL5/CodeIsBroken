using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Materials
{
    Wood = 1 << 0,
    Steel = 1 << 1,
}

public class Item : MonoBehaviour
{
    public Materials materials;
    [field: SerializeField] public List<Modification> mods { get; private set; } = new List<Modification>();

    public MeshRenderer artRenderer;
    public static List<Item> items = new List<Item>();

    public bool destroyOnPause = true;

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
        if(!HasMod(modification))
            mods.Add(modification);
    }

    public bool HasMod<T>(T toCompareWith) where T : Modification
    {
        foreach (var mod in mods)
        {
            if (mod is T)
            {
                if(mod.Compare(toCompareWith))
                    return true;
            }
        }
        return false;
    }
}
