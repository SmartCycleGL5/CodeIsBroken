using System;
using System.Collections.Generic;
using UnityEngine;

public class ContractSystem : MonoBehaviour
{
    public static Contract ActiveContract;
    private void Start()
    {
        ActiveContract = NewContract();
    }

    public Contract NewContract()
    {
        Contract contract = new Contract("cool contract");
        contract.onFinished += FinishedContract;

        return contract;
    }
    void FinishedContract(Contract contract)
    {
        if (contract != ActiveContract) return;

        ActiveContract = NewContract();
    }
}

public class Contract
{
    public string contractName;

    public BaseMaterial baseMaterial;
    public Modification[] modifications;

    public int amount;

    public Action<Contract> onFinished;

    public Contract(string name)
    {
        contractName = name;

        baseMaterial = new BaseMaterial(BaseMaterial.Materials.Wood);
        
        List<Modification> mods = new List<Modification>();

        mods.Add(Modification.RandomModification());

        modifications = mods.ToArray();

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
        if (!item.material.Compare(baseMaterial)) return false;

        foreach (var mod in modifications)
        {
            if (!item.HasMod(mod)) return false;
        }

        return true;
    }
}
