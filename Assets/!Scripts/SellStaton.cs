
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellStaton : MonoBehaviour
{
    [SerializeField] List<Transform> sellPoints = new();
    [SerializeField] private TextMeshProUGUI text;
    private int cubeSold;

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
                continue;
            }

            if (!cell.TryGetComponent(out Conveyor conveyor))
            {
                Debug.Log("[SellStation] Cell not conveyor");
                continue;
            }

            if(conveyor.RemoveItem(out Item removedItem))
            {
                //Debug.Log("[SellStation] Sold " +  removedItem);
                cubeSold++;
                Destroy(removedItem.gameObject);
            }
        }

        if (cubeSold < 10)
        {
            text.text = "Try building a conveyor belt going into the selling station and a material tube at the other end. Use GetMaterial in update to start spawning cubes!";
        }
        else if (cubeSold > 15)
        {
            text.text = "Try putting a painter in between two conveyors. In the update, use the command Paint(); and chose between red or blue";
        }
        
        else if (cubeSold > 25)
        {
            text.text = "Create a crane between two conveyors and make it rotate and lift up items";
        }
        else if (cubeSold > 40)
        {
            text.text = "Create two lines going into the seller and make them two different colors";
        }
    }
}
