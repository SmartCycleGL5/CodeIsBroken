using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContractSystem : MonoBehaviour
{
    public static ContractSystem instance;

    [SerializeField] Item DisplayItem;
    [SerializeField] TMP_Text amoundDisplay;
    public static Contract ActiveContract;
    private void Start()
    {
        SetContract(NewContract());
        instance = this;
    }
    private void Update()
    {
        amoundDisplay.text = "X" + ActiveContract.amount;
        Debug.Log(ActiveContract.amount);
    }
    public void SetContract(Contract contract)
    {
        ActiveContract = contract;
    }

    public Contract NewContract()
    {
        Contract contract = new Contract("cool contract", DisplayItem);
        contract.onFinished += FinishedContract;

        return contract;
    }
    void FinishedContract(Contract contract)
    {
        if (contract != ActiveContract) return;

        SetContract(NewContract());
    }
}

public class Contract
{
    public string contractName;

    public Item requestedItem;

    public int amount;

    public Action<Contract> onFinished;

    public Contract(string name, Item item)
    {
        requestedItem = item;
        contractName = name;

        requestedItem.material = new BaseMaterial(BaseMaterial.Materials.Wood);

        for (int i = 0; i < 1; i++)
        {
            Modification.RandomModification(requestedItem);
        }

        amount = 10;
    }
    public void Progress()
    {
        amount--;

        if (amount <= 0)
        {
            Finish();
        }
    }
    public void Finish()
    {
        PlayerProgression.GiveXP(10);
        onFinished?.Invoke(this);
    }

    public bool SatisfiesContract(Item item)
    {
        if (!item.material.Compare(requestedItem.material)) return false;

        foreach (var mod in requestedItem.mods)
        {
            if (!item.HasMod(mod)) return false;
        }

        return true;
    }
}
