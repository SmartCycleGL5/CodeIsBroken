
using UnityEngine;


public class BuildingButton : MonoBehaviour
{
    public BuildingSelector blockSelector;
    public GameObject buildingPrefab;

    public void OnSelected()
    {
        Debug.Log(buildingPrefab.name);
        blockSelector.UpdateBuildingBlock(buildingPrefab);
    }
}