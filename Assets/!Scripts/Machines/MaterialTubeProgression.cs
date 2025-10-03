using AYellowpaper.SerializedCollections;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTubeProgression : MonoBehaviour
{
    [SerializedDictionary("Level", "Tube Object")]
    public SerializedDictionary<int, GameObject> unlockLevel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnLevelUp(1);
        PlayerProgression.onLevelUp += OnLevelUp;
    }

    public void OnLevelUp(int level)
    {
        foreach (KeyValuePair<int, GameObject> entry in unlockLevel)
        {
            if(entry.Key != level) continue;
            entry.Value.SetActive(true);
            Vector3 position = entry.Value.transform.position;
            position.y = 0.5f;
            entry.Value.transform.DOMove(position, 1);
        }
    }
}
