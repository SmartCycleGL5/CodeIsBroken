using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public BaseMaterial material;
    [field: SerializeField] public List<Modification> mods { get; private set; } = new List<Modification>();

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
        Steel
    }

    public Materials type;

    public BaseMaterial(Materials type)
    {
        this.type = type;
    }
}
