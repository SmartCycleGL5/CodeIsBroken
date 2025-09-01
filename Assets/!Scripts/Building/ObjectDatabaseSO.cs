using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class ObjectDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectData;
}

public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }

    
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    
}
