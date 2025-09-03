using UnityEngine;
using System;

public class BuildingSelector : MonoBehaviour
{
    public static event Action<GameObject> OnChangedBuilding;
    
    [SerializeField] private GameObject[] folderObjects;
    [SerializeField] private GameObject buildingMenuPanel;
    [SerializeField] private GameObject button;
    
    void Start()
    {
        folderObjects = Resources.LoadAll<GameObject>("BuildingBlocks");
        foreach (var t in folderObjects)
        {
            GameObject newButton = Instantiate(button, buildingMenuPanel.transform);
            BuildingButton buildingButton = newButton.GetComponent<BuildingButton>();
            buildingButton.buildingPrefab = t;
            buildingButton.buttonText.text = t.name;
            buildingButton.blockSelector = this;
        }
    }

    public void UpdateBuildingBlock(GameObject buildingBlock)
    {
        OnChangedBuilding?.Invoke(buildingBlock);
    }

}