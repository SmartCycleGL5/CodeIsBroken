using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using AYellowpaper.SerializedCollections;

public class GridBuilder : MonoBehaviour
{
    public static GridBuilder instance;

    Vector2 _lastPosition;
    [SerializeField] Grid grid;
    [SerializeField] GhostBuilding ghostBuilding;
    [SerializedDictionary("Position", "Object in position")]
    public SerializedDictionary<Vector2Int, GameObject> gridObjects = new();

    private void Start()
    {
        instance = this;
    }

    // Get the grid cell mouse is hovering over
    public Vector2 GetGridPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray ,out RaycastHit hit, 100f))
        {
            Vector3Int gridPosition = grid.WorldToCell(hit.point);
            Vector3 cellCenter = grid.GetCellCenterWorld(gridPosition);
            _lastPosition = new Vector2(cellCenter.x, cellCenter.z);
        }
        return _lastPosition;
    }
    
    // Checks if grid position is available.
    public bool IsValidPosition(List<Vector3> positions)
    {
        foreach (var pos in positions)
        {
            Vector3Int cellPos = grid.WorldToCell(pos);
            if (gridObjects.ContainsKey(new Vector2Int(cellPos.x, cellPos.z)))
            {
                return false;
            }
        }
        return true;
    }

    //Places Building
    public void PlaceBuilding(GameObject building, Quaternion rotation)
    {
        Vector2 gridPosition = GetGridPosition();
        GameObject newBuilding = Instantiate(building, new Vector3(gridPosition.x,0,gridPosition.y), rotation);
        Building  buildingData = newBuilding.GetComponent<Building>();
        
        foreach (var cell in buildingData.GetBuildingPositions())
        {
            Vector3Int cellPosition = grid.WorldToCell(cell);
            gridObjects.Add(new Vector2Int(cellPosition.x,cellPosition.z), newBuilding);
        }
    }

    public void RemoveBuilding()
    {
        Vector2 cellCenter = GetGridPosition();
        Vector3Int gridPosition = grid.WorldToCell(new Vector3(cellCenter.x,0,cellCenter.y));
        if (gridObjects.TryGetValue(new Vector2Int(gridPosition.x, gridPosition.z), out var building))
        {
            Building buildingData = building.GetComponent<Building>();
            foreach (var pos in buildingData.GetBuildingPositions())
            {
                Vector3Int gridPos = grid.WorldToCell(pos);
                gridObjects.Remove(new Vector2Int(gridPos.x, gridPos.z));
            }
            Destroy(building);
        }
    }
    public GameObject LookUpCell(Vector3 pos)
    {
        Vector3Int cellPos = grid.WorldToCell(pos);
        if (gridObjects.TryGetValue(new Vector2Int(cellPos.x, cellPos.z), out GameObject building))
        {
            //Debug.Log("Looked up: " + new Vector2(cellPos.x, cellPos.z)+" - "+building);
            return building;
        }
        return null;
    }
}
