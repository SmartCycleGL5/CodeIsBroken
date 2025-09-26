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
