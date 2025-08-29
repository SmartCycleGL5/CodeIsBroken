using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject mouseIndicator;
    [SerializeField] private GameObject cellIndicator;
    [SerializeField] private BuildingInput inputManager;
    [SerializeField] private Grid grid;
    private Vector3 blockOffset = new Vector3(0.5f, 0, 0.5f);
    bool isBuilding = false;

    private void Start()
    {
        //input
        BuildingInput.OnClickedLeft += PlaceBlock;
        BuildingInput.Rotate += RotateCellIndicator;
        BuildingInput.DisableBuilding += DisableBuilding;
        
        //Updates when new building block is selected
        BuildingBlockSelector.OnChangedBuilding += UpdateCellIndicator;
    }


    
    public Vector3Int GetCellPosition()
    {
        Vector3 mousePosition = inputManager.GetMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        return gridPosition;
    }

    private void GhostBlock()
    {
        cellIndicator.transform.position = grid.CellToWorld(GetCellPosition())+blockOffset;
    }

    private void UpdateCellIndicator(GameObject model)
    {
        Destroy(cellIndicator);
        isBuilding = true;
        cellIndicator = Instantiate(model);
        
    }

    void RotateCellIndicator()
    {
        if(!isBuilding) return;
        cellIndicator.transform.Rotate(Vector3.up, 90);
        
        
    }

    void PlaceBlock()
    {
        if (!isBuilding)return;
        Instantiate(cellIndicator, grid.CellToWorld(GetCellPosition())+blockOffset, cellIndicator.transform.rotation);
    }

    void DisableBuilding()
    {
        Destroy(cellIndicator);
        isBuilding = false;
    }

    private void Update()
    {
        if (isBuilding)
        {
            GhostBlock();
        }
    }
    
}
