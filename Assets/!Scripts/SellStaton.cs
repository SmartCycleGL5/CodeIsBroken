
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SellStaton : MonoBehaviour
{
    [SerializeField] List<Transform> sellPoints = new();

    Modification FavoredModification;
    int desiredAmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Tick.OnTick += CheckConveyor;
        SetDesire();
    }
    void OnDestroy()
    {
        Tick.OnTick -= CheckConveyor;
    }

    void SetDesire()
    {
        FavoredModification = Modification.RandomModification();
        desiredAmount = Random.Range(10, 100);
        Debug.Log(((ModColor)FavoredModification).color);
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
                SellItem(removedItem);
            }
        }

        if (desiredAmount <= 0)
            SetDesire();

    }

    void SellItem(Item toSell)
    {
        if (toSell.HasMod(FavoredModification))
        {
            PlayerProgression.GiveXP(3);
        }
        else
        {
            PlayerProgression.GiveXP(1);
        }

        Debug.Log(PlayerProgression.experience);

        Destroy(toSell.gameObject);
    }
}
