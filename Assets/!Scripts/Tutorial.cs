using WindowSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Tutorial : MonoBehaviour
{
    private int tutorialLevel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] List<string> tutorialPrompts;

    [SerializeField] private GameObject buildingAreaPrefab;
    [SerializeField] private GameObject buildingAreaPrefab2;
    [SerializeField] private GameObject replaceBlock;
    [SerializeField] private GameObject buttons;
    [SerializeField] private BuildingSO painterSO;
    [SerializeField] private BuildingSelector selector;

    [SerializeField] private List<GameObject> scriptsToDelete;


    private void Start()
    {
        Tick.OnStartingTick += TickStart;
    }

    void Update()
    {
        Debug.Log("TutorialRunning");
        tutorialText.text = tutorialPrompts[tutorialLevel];
        switch (tutorialLevel)
        {
            //Prompt user to press B
            case 0:
                if (Keyboard.current.bKey.wasPressedThisFrame) tutorialLevel++;
                return;
            case 1:
                buildingAreaPrefab.SetActive(true);
                if (CheckBuildingArea()) tutorialLevel++;
                return;
            case 2:
                buildingAreaPrefab.SetActive(false);
                buildingAreaPrefab2.SetActive(true);
                if (CheckBuildingArea2()) tutorialLevel++;
                return;
            case 3:
                buildingAreaPrefab2.SetActive(false);
                replaceBlock.SetActive(true);
                if (!CheckIfCorrectBlock(replaceBlock)) tutorialLevel++;
                return;
            case 4:
                selector.OnLevelUp(2);
                if (CheckIfCorrectBlock(replaceBlock)) tutorialLevel++;
                return;
            case 5:
                replaceBlock.SetActive(false);
                if (Terminal.focused) tutorialLevel++;
                return;
            case 6:
                buttons.SetActive(true);
                return;
        }

    }

    void TickStart()
    {
        tutorialLevel++;
    }

    bool CheckBuildingArea()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = GridBuilder.instance.LookUpCell(new Vector3(0, 0, i));
            if (obj == null) return false;
        }
        return true;

    }
    bool CheckBuildingArea2()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = GridBuilder.instance.LookUpCell(new Vector3(-i, 0, 0));
            if (obj == null) return false;
        }
        return true;

    }
    bool CheckIfCorrectBlock(GameObject gameObj)
    {
        GameObject cell = GridBuilder.instance.LookUpCell(gameObj.transform.position);
        if (cell == null) return false;
        return true;
    }

    public void PlayAgain()
    {
        ScriptManager.StopMachines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Continue()
    {
        foreach (var scripts in scriptsToDelete)
        {
            Destroy(scripts.gameObject);
        }
        ScriptManager.StopMachines();
        SceneManager.LoadScene("GameScene");
        
    }
}
