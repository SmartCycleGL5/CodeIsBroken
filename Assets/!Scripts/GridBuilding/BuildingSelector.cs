using UnityEngine;
using System.Collections.Generic;
using System;
using NUnit.Framework;

public class BuildingSelector : MonoBehaviour
{
    public static event Action<GameObject> OnChangedBuilding;

    [SerializeField] List<BuildingSO> buildings;
    //[SerializeField] private GameObject[] folderObjects;
    [SerializeField] private GameObject buildingMenuPanel;
    [SerializeField] private GameObject button;
    
    void Start()
    {
        OnLevelUp(1);
        PlayerProgression.onLevelUp += OnLevelUp;
    }

    public void OnLevelUp(int level)
    {
        List<BuildingSO> buildingsToRemove = new();
        Debug.Log("LevelUp: " + level);
        for (int i = 0; i < buildings.Count; i++)
        {
            var building = buildings[i];
            if (building.levelToUnlock > level) continue;
            buildingsToRemove.Add(building);
            GameObject newButton = Instantiate(button, buildingMenuPanel.transform);
            BuildingButton buildingButton = newButton.GetComponent<BuildingButton>();
            buildingButton.buildingPrefab = building.buildingPrefab;
            buildingButton.buttonText.text = building.buildingName;
            buildingButton.blockSelector = this;
        }

        foreach (var building in buildingsToRemove)
        {
            buildings.Remove(building);
        }
        buildingsToRemove.Clear();
    }




    public void UpdateBuildingBlock(GameObject buildingBlock)
    {
        OnChangedBuilding?.Invoke(buildingBlock);
    }



}