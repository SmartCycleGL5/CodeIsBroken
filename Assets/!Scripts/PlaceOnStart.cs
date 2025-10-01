using UnityEngine;
using System.Collections.Generic;

public class PlaceOnStart : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<Vector3> position = GetComponent<Building>().GetBuildingPositions();
        GridBuilder.instance.PlaceBuilding(this.gameObject, gameObject.transform.rotation);
    }
}
