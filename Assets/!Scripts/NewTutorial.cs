using DG.Tweening;
using ScriptEditor;
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

    private bool subscribed = false;
    private string contractName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiDocument.rootVisualElement.Q<VisualElement>("BuildingMenu").visible = false;
        player = PlayerInputs.instance.gameObject.transform;
        startPosition = PlayerInputs.instance.gameObject.transform.position;
        label = uiDocument.rootVisualElement.Q<Label>("TutorialText");

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
            case 3:
                ProgrammingTutorial();
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
                uiDocument.rootVisualElement.Q<VisualElement>("BuildingMenu").visible = true;
                level++;
                return;
        }
    }

    void BuildingTutorial()
    {
        uiDocument.rootVisualElement.Q<Label>("PressToContinue").visible = false;
        switch (buildingIndex)
        {
            case 0:
                label.text = "Press B to open the building menu or click the building button at the bottom of your screen.\n\nUse WASD to move and QE to rotate.";
                player.DOMove(startPosition, 0.2f);
                player.transform.DORotate(new Vector3(0, 0, 0), 0.2f);
                PlayerInputs.instance.enabled = true;
                if (!BuildingSelector.instance.isBuilding)
                    buildingIndex++;
                return;
            case 1:
                label.text = "Try selecting conveyors and building a line from the MaterialTubes to the Selling station. Close the menu and press run when completed!\n\n Use left click to place an item, R to rotate and Right click to remove a building.";
                return;
            case 2:
                label.text = "Good job! This is the contract system. Select a product you would like to craft.";
                if (ContractSystem.ActiveContract != null)
                {
                    contractName = ContractSystem.ActiveContract.requestedItem.materials.ToString();
                    level++;
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
                if (Terminal.focused)
                { buildingIndex++; }
                break;
            case 2:
                label.text = $"Try writing ChangeMaterial({contractName}) and run it. Replace a conveyor with a painter and write Paint() in the OnTick section. \n\nPress J to open the journal and check the colors you can paint.";
                if (!subscribed)
                {
                    Debug.Log("Subscribed");
                    ContractSystem.ActiveContract.onFinished += OnFinishedContract;
                    subscribed = true;
                }
                break;
            case 3:
                label.text = "Good job! You are on your own now. Press J to open the journal. The journal will tell you how to use the machines you unlock.";
                if (Keyboard.current.jKey.wasPressedThisFrame)
                {
                    buildingIndex++;
                }
                break;
            case 4:
                uiDocument.rootVisualElement.Q<VisualElement>("Tutorial").visible = false;
                this.enabled = false;
                break;
        }
    }


    void OnFinishedContract(Contract contract)
    {
        Debug.Log("Finished");
        buildingIndex++;
        ContractSystem.ActiveContract.onFinished -= OnFinishedContract;
    }
}
