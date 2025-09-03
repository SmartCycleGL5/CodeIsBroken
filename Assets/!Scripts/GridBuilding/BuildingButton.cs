using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public BuildingSelector blockSelector;
    public TextMeshProUGUI buttonText;
    public GameObject buildingPrefab;

    public void OnSelected()
    {
        Debug.Log(buildingPrefab.name);
        blockSelector.UpdateBuildingBlock(buildingPrefab);
    }
}