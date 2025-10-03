using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO")]
public class BuildingSO : ScriptableObject
{
    public GameObject buildingPrefab;
    public string buildingName;
    public Sprite buildingImage;
    public string buildingDesctiption;

    public int levelToUnlock;
    public bool isUnloced;
}
