using UnityEngine;

[CreateAssetMenu(fileName = "BuildingSO")]
public class BuildingSO : ScriptableObject
{
    public GameObject buildingPrefab;
    public string buildingName;
    public Sprite buildingImage;
    [NaughtyAttributes.ResizableTextArea]
    public string buildingDesctiption;

    public int levelToUnlock;
    public bool isUnlocked => levelToUnlock <= PlayerProgression.Level;
    public int sortingOrder = 1;
    public Sprite icon;
}
