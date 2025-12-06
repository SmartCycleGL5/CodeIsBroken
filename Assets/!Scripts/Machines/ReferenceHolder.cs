using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class ReferenceHolder : MonoBehaviour
{
    [SerializedDictionary("Name", "GameObjects")]
    [SerializeField] SerializedDictionary<string, GameObject> references = new();

    public GameObject GetReference(string name)
    {
        return references[name];
    }

}
