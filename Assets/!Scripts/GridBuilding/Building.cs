using System.Collections.Generic;

using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildingCells;
    [SerializeField] private List<MonoBehaviour> scriptsToActivate;

    public List<Vector3> GetBuildingPositions()
    {
        //Debug.Log("Building System looked up position");
        List<Vector3> positions = new();
        foreach (var cell in buildingCells)
        {
            positions.Add(cell.transform.position);
        }
        return positions;
    }
    public void Built()
    {
        foreach(MonoBehaviour script in scriptsToActivate)
        {
            script.enabled = true;
        }
    }
}
