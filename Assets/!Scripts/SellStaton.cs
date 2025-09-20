
using System.Collections.Generic;
using UnityEngine;

public class SellStaton : MonoBehaviour
{
    [SerializeField] List<Transform> sellPoints = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tick.OnTick += CheckConveyor;
    }
    void OnDestroy()
    {
        Tick.OnTick -= CheckConveyor;
    }

    void CheckConveyor()
    {
        foreach (var location in sellPoints)
        {
            GameObject cell = GridBuilder.instance.LookUpCell(location.position);

            if (cell == null)
            {
                Debug.Log("[SellStation] Nothing in cell");
                return;
            }

            if (!cell.TryGetComponent(out Conveyor conveyor))
            {
                Debug.Log("[SellStation] Cell not conveyor");
                return;
            }

            if(conveyor.RemoveItem(out Item removedItem))
            {
                Debug.Log("[SellStation] Sold " +  removedItem);
                Destroy(removedItem.gameObject);
            }
        }
    }
}
