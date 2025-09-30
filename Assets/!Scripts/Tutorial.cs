using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tutorial : MonoBehaviour
{
    private int tutorialLevel;
    [SerializeField] private TextMeshProUGUI tutorialText;
    [SerializeField] List<string> tutorialPrompts;

    [SerializeField] private GameObject buildingAreaPrefab;
    [SerializeField] private GameObject buildingAreaPrefab2;


     void Update()
    {
        tutorialText.text = tutorialPrompts[tutorialLevel];
        switch (tutorialLevel)
        {
            //Prompt user to press B
            case 0:
                if (Keyboard.current.bKey.wasPressedThisFrame) tutorialLevel++;
                return;
            case 1:
                buildingAreaPrefab.SetActive(true);
                if(CheckBuildingArea()) tutorialLevel++;
                return;
            case 2:
                buildingAreaPrefab.SetActive(false);
                buildingAreaPrefab2.SetActive(true);
                if(CheckBuildingArea2()) tutorialLevel++;
                return;
            case 3:
                buildingAreaPrefab2.SetActive(false);
                
                return;
        }
       
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
}
