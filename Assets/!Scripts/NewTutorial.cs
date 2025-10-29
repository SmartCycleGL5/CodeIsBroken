using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
public class NewTutorial : MonoBehaviour
{
    
    [Header("Positions")] 
    [SerializeField] private Transform materialTubes;
    [SerializeField] private Transform sellingStation;
    
    [Header("References")]
    [SerializeField] UIDocument uiDocument;
    
    private Label label;
    private int level;
    private int buttonPressIndex;
    private Vector3 startPosition;
    private Transform player;
    private int buildingIndex;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = PlayerInputs.instance.gameObject.transform;
        startPosition = PlayerInputs.instance.gameObject.transform.position;
        label= uiDocument.rootVisualElement.Q<Label>("TutorialText");
        
        PlayerProgression.onLevelUp += OnLevelUp;
        // Disable movement and building for player.
        PlayerInputs.instance.enabled = false;
        
        OnLevelUp(1);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            buttonPressIndex++;
        }
        
        switch (level)
        {
            case 1:
                ShowAround();
                return;
            case 2:
                BuildingTutorial();
                return;
        }
    }

    void OnLevelUp(int level)
    {
        this.level = level;
        if (buildingIndex == 1)
        {
            Debug.Log("all items deliverd");
            buildingIndex++;
        }
    }

    void ShowAround()
    {
        switch (buttonPressIndex)
        {
            case 0:
                label.text = "Welcome to your factory! Let me show you around.";
                return;
            case 1:
                label.text = "This is the material tube, It will give you items to create products with.";
                player.DOMove(materialTubes.position, 0.2f);
                return;
            case 2:
                label.text = "This is the selling station, bring item over here to sell them";
                player.DOMove(sellingStation.position, 0.2f);
                player.transform.DORotate(sellingStation.transform.localRotation.eulerAngles, 0.2f);
                return;
            case 3:
                level++;
                return;
        }
    }

    void BuildingTutorial()
    {
        switch (buildingIndex)
        {
            case 0:
                label.text = "Press B to open the building menu or click the building button at the bottom of your screen.";
                player.DOMove(startPosition, 0.2f);
                player.transform.DORotate(new Vector3(0,0,0), 0.2f);
                PlayerInputs.instance.enabled = true;
                if (!BuildingSelector.instance.isBuilding)
                    buildingIndex++;
                return;
            case 1:
                label.text = "Try selecting conveyors and building a line between the MaterialTubes and the Selling station. Close the menu and press run when completed! Use left click to place an item, R to rotate and Right click to remove a building.";
                return;
            case 2:
                label.text = "Good job! This is the contract system. Select a product you would like to craft.";
                if (ContractSystem.ActiveContract != null)
                {
                    buildingIndex++;
                }
                return;
            case 3:
                label.text = "Lets try some programming!";
                if (ContractSystem.ActiveContract != null)
                {
                    buildingIndex++;
                }
                return;
        }
    }

    void ProgrammingTutorial()
    {
        switch (buildingIndex)
        {
            case 0:
                label.text = "Lets try some programming! Click on the material tube and press the terminal symbol. Try writing something inside the terminal!";
                break;
            case 2:
                label.text = " ";
                break;
        }
    }
}
