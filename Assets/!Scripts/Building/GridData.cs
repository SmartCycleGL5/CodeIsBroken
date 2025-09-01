using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridData : MonoBehaviour
{
    Dictionary<Vector2Int, GameObject> data = new Dictionary<Vector2Int, GameObject>();

    public bool IsCellOccupied(List<Vector2Int> gridPosition)
    {
        foreach (var grid in gridPosition)
        {
            if (data.ContainsKey(grid))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }


}
