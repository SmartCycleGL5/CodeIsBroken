using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GhostBuilding : MonoBehaviour
{
    [SerializeField] Material positiveMaterial;
    [SerializeField] Material negativeMaterial;

    private List<Renderer> renderers = new();
    
    [SerializeField] GridBuilder gridBuilder;
    [SerializeField] GameObject ghostPrefab;
    private GameObject prefabToBuild;
    
    
    public bool isBuilding = false;

    private Building building;
    private bool canPlace;

    private void Start()
    {
        BuildingSelector.OnChangedBuilding += SwapGhostBlock;
    }

    public void PlayerUpdate()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Destroy(ghostPrefab);
            isBuilding = false;
        }
        if (!isBuilding) return;
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RotateBuilding();
        }
        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace)
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            gridBuilder.PlaceBuilding(prefabToBuild, ghostPrefab.transform.Find("Wrapper").rotation);
        }
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            gridBuilder.RemoveBuilding();
        }
        
        if (isBuilding)
        {
            SetGhostPosition();
            BuildingCollisions();
        }
    }

    // Updates position of GhostBlock
    void SetGhostPosition()
    {
        Vector2 gridPosition = gridBuilder.GetGridPosition();
        ghostPrefab.transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);

    }

    void RotateBuilding()
    {
        ghostPrefab.transform.Find("Wrapper").Rotate(0,90,0);
    }
    
    // Checks if cell is available
    void BuildingCollisions()
    {   
        building = ghostPrefab.GetComponent<Building>();
        canPlace = gridBuilder.IsValidPosition(building.GetBuildingPositions());
        foreach(var renderer in renderers)
        {
            renderer.material = canPlace? positiveMaterial : negativeMaterial;
        }
    }

    // Used for enable or disable building
    public void SetBuildingStatus(bool buildingStatus)
    {
        isBuilding = buildingStatus;
    }
    
    // Change building block
    private void SwapGhostBlock(GameObject newGhost)
    {
        SetBuildingStatus(true);
        prefabToBuild = newGhost;
        ghostPrefab = Instantiate(newGhost);
        building = ghostPrefab.GetComponent<Building>();
        renderers.AddRange(ghostPrefab.GetComponentsInChildren<Renderer>());
    }

    public void DestroyGhost()
    {
        Destroy(ghostPrefab);
    }
}
