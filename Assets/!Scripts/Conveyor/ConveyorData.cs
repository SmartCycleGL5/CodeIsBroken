using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ConveyorData : MonoBehaviour
{
    [SerializeField] Grid grid;
    Dictionary<Vector2Int, Conveyor> data = new();

    private void Start()
    {
        //grid = FindAnyObjectByType<Grid>();
    }

    public Vector2Int PositionToCell(Vector2 pos)
    {
        Vector3Int worldCellPos= grid.WorldToCell(new Vector3(pos.x, 0, pos.y));
        return new Vector2Int(worldCellPos.x, worldCellPos.z);
    }

    public void AddConveyor(Vector2 position, Conveyor conveyor)
    {
        data.Add(PositionToCell(position), conveyor);
        Debug.Log(PositionToCell(position)+" "+conveyor.gameObject.name);
    }
    public Conveyor GetConveyorOnCell(Vector2 cell)
    {
        if (data.TryGetValue(PositionToCell(cell), out Conveyor result))
        {
            return result;
        }
        else { return null; }
    }
}
