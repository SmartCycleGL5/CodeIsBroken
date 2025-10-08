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
    private List<Material> originalMaterial = new();
    
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
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Debug.Log("Trying to remove building");
            gridBuilder.RemoveBuilding();
        }
        if (!isBuilding) return;
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RotateBuilding();
        }
        // Block placement when over UI
        Debug.Log(EventSystem.current.IsPointerOverGameObject());

        if (EventSystem.current.IsPointerOverGameObject()) return;
        

        if (Mouse.current.leftButton.wasPressedThisFrame && canPlace)
        {
            gridBuilder.PlaceBuilding(prefabToBuild, ghostPrefab.transform.Find("Wrapper").rotation);
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
            if (canPlace)
            {
                renderer.material.color = new Color(0, 0.5f, 0, 1);
            }
            else
            {
                renderer.material.color = new Color(0.5f, 0, 0, 1);
            }
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
        DestroyGhost();
        ghostPrefab = Instantiate(newGhost, new Vector3(0,1,0), Quaternion.identity);
        building = ghostPrefab.GetComponent<Building>();
        renderers.Clear();
        renderers.AddRange(ghostPrefab.GetComponentsInChildren<Renderer>());
        originalMaterial.AddRange(ghostPrefab.GetComponentInChildren<Renderer>().materials);
        foreach (var renderer in renderers)
        {
            renderer.material.color = new Color(0.5f, 0.5f, 0, 1);
        }
    }

    public void DestroyGhost()
    {
        Destroy(ghostPrefab);
    }

    private void OnDestroy()
    {
        BuildingSelector.OnChangedBuilding -= SwapGhostBlock;
    }
}
