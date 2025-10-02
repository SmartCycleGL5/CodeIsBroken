using System;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum Materials
{
    Wood = 1 << 0,
    Iron = 1 << 1,
}

[Serializable]
public class ItemDefinition : IEquatable<ItemDefinition>
{
    public Materials materials;

    [field: SerializeField] public List<Modification> mods { get; private set; } = new();
    public Action<Modification> modified;

    public ItemDefinition(Materials materials, List<Modification> modifications = null)
    {
        this.materials = materials;

        if(modifications != null)
            mods = modifications;
    }

    public void Modify(Modification modification)
    {
        if (HasMod(modification)) return;

        mods.Add(modification);
        modified?.Invoke(modification);
    }
    public bool HasMod<T>(T toCompareWith) where T : Modification
    {
        foreach (var mod in mods)
        {
            if (mod is T)
            {
                if (mod.Compare(toCompareWith))
                    return true;
            }
        }
        return false;
    }

    public bool Equals(ItemDefinition other)
    {
        if (materials != other.materials) return false;
        if (mods.Count != other.mods.Count) return false;

        int modsSatisfied = 0;

        foreach (var mod in other.mods)
        {
            if (HasMod(mod)) modsSatisfied++;
        }

        return modsSatisfied == other.mods.Count;
    }
}

public class Item : MonoBehaviour
{
    public ItemDefinition definition = new(Materials.Wood);

    public MeshRenderer artRenderer;
    public static List<Item> items = new List<Item>();

    public bool destroyOnPause = true;

    private void Start()
    {
        artRenderer.material.SetFloat("_Rng", UnityEngine.Random.Range(10, 100));
        items.Add(this);

        foreach (var mod in definition.mods)
        {
            mod.Apply(this);
        }

        definition.modified += ApplyModifications;
    }
    private void OnDestroy()
    {
        items.Remove(this);
        definition.modified -= ApplyModifications;
    }

    void ApplyModifications(Modification mod)
    {
        mod.Apply(this);
    }
}
