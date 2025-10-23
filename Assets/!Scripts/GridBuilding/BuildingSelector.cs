using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using Button = UnityEngine.UIElements.Button;

public class BuildingSelector : MonoBehaviour
{
    public static event Action<GameObject> OnChangedBuilding;

    [SerializeField] private List<BuildingSO> buildings;
    [SerializeField] private VisualTreeAsset buildingButtonTemplate;
    private VisualElement buildingMenuPanel;
    [SerializeField] UIDocument uiDoc;

    void Start()
    {
        var root = uiDoc.rootVisualElement;
        
        buildingMenuPanel = uiDoc.rootVisualElement.Q<VisualElement>("BuildingMenu");
        
        OnLevelUp(1);
        PlayerProgression.onLevelUp += OnLevelUp;
    }

    public void OnLevelUp(int level)
    {
        List<BuildingSO> unlocked = new();

        foreach (var building in buildings)
        {
            if (building.levelToUnlock > level) continue;
            unlocked.Add(building);
            
            var newButton = buildingButtonTemplate.CloneTree();
            Debug.Log(newButton);
            var button = newButton.Q<Button>("Button");
            Debug.Log(button);
            
            button.text = building.buildingName;
            
            button.clicked += () => UpdateBuildingBlock(building.buildingPrefab);
            
            buildingMenuPanel.Add(newButton);
        }
        
        foreach (var b in unlocked)
            buildings.Remove(b);
    }

    public void UpdateBuildingBlock(GameObject buildingBlock)
    {
        OnChangedBuilding?.Invoke(buildingBlock);
    }
}