using UnityEngine;
using System.Collections.Generic;

public class PlaceOnStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("PlaceBuilding", 0.5f);
        
    }

    void PlaceBuilding()
    {
        Debug.Log("PlacedMaterialTube: " + gameObject);
        GridBuilder.instance.AddBuildingToGrid(transform.position ,this.gameObject);
    }
}
