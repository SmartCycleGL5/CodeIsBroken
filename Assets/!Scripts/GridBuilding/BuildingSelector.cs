using ScriptEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class BuildingSelector : MonoBehaviour
{
    public static BuildingSelector instance;
    public static event Action<GameObject> OnChangedBuilding;

    [SerializeField] private List<BuildingSO> buildings;
    [SerializeField] private VisualTreeAsset buildingButtonTemplate;
    private VisualElement buildingMenuPanel;
    [SerializeField] UIDocument uiDoc;
    public bool isBuilding = true;

    [SerializeField] private Sprite arrowDown;
    [SerializeField] private Sprite arrowUp;
    private VisualElement menu;
    private Button moveButton;



    void Start()
    {
        instance = this;
        var root = uiDoc.rootVisualElement;

        buildingMenuPanel = root.Q<VisualElement>("ButtonHolder");

        OnLevelUp(1);
        PlayerProgression.onLevelUp += OnLevelUp;

        //Subscribe toggle button to method

        menu = root.Q<VisualElement>("BuildingMenu");
        moveButton = root.Q<Button>("BuildingMenuToggle");
        moveButton.style.backgroundImage = new StyleBackground(arrowUp);
        moveButton.clicked += ToggleMenu;
    }

    private void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame && !Terminal.focused)
        {
            ToggleMenu();
        }
    }

    public void OnLevelUp(int level)
    {
        List<BuildingSO> unlocked = new();

        foreach (var building in buildings)
        {
            if (building.levelToUnlock > level) continue;
            unlocked.Add(building);

            var newButton = buildingButtonTemplate.CloneTree();
            var button = newButton.Q<Button>("Button");

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

    private void ToggleMenu()
    {
        if (!isBuilding)
        {
            moveButton.style.backgroundImage = new StyleBackground(arrowUp);
            menu.style.translate = new StyleTranslate(new Translate(0, 0, 0));
            PlayerInputs.instance.BuildingMenu(isBuilding);
        }
        else
        {
            moveButton.style.backgroundImage = new StyleBackground(arrowDown);
            menu.style.translate = new StyleTranslate(new Translate(0, -282, 0));
            PlayerInputs.instance.BuildingMenu(isBuilding);
        }


        isBuilding = !isBuilding;
    }
}