
using System.Collections.Generic;
using CodeIsBroken.Contract;
using CodeIsBroken.Product;
using TMPro;
using UnityEngine;

public class SellStaton : MonoBehaviour
{
    [SerializeField] List<Transform> sellPoints = new();
    [SerializeField] private bool isTutorial;


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
                //Debug.Log("[SellStation] Nothing in cell");
                continue;
            }

            if (!cell.TryGetComponent(out Conveyor conveyor))
            {
                //Debug.Log("[SellStation] Cell not conveyor");
                continue;
            }

            if(conveyor.RemoveItem(out Item removedItem))
            {
                SellItem(removedItem);
            }
        }

    }

    void SellItem(Item toSell)
    {
        if(ContractManager.ActiveContract != null)
        {
            ContractManager.ActiveContract.TryProgressContract(toSell);
        }
        
        //Remove is tutorial
        if (!isTutorial)
        {
            PlayerProgression.GiveXP(1);
        }
        

        Destroy(toSell.gameObject);
    }
}
