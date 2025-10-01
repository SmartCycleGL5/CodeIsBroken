using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ContractSystem : MonoBehaviour
{
    public static ContractSystem instance;

    [SerializeField] Transform displayPos;
    Item displayItem;
    [SerializeField] TMP_Text amoundDisplay;
    public static Contract ActiveContract;
    private void Start()
    {
        NewContract();
        instance = this;
    }
    private void Update()
    {
        if (ActiveContract == null) return;
        amoundDisplay.text = "X" + ActiveContract.amount;
        Debug.Log(ActiveContract.amount);
    }

    void CreateDisplayItem()
    {
        if (displayItem != null) Destroy(displayItem.gameObject);

        displayItem = Instantiate(MaterialManager.Instance.items[Materials.Wood], displayPos);
        displayItem.destroyOnPause = false;
    }

    public void NewContract()
    {
        CreateDisplayItem();

        Contract contract = new Contract("cool contract", displayItem);
        contract.onFinished += FinishedContract;

        ActiveContract = contract;
    }
    void FinishedContract(Contract contract)
    {
        if (contract != ActiveContract) return;

        NewContract();
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

        for (int i = 0; i < 2; i++)
        {
            Modification.RandomModification(requestedItem);
        }

        amount = UnityEngine.Random.Range(10, 20);
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
        if (item.materials != requestedItem.materials) return false;
        if (item.mods.Count != requestedItem.mods.Count) return false;

        int modsSatisfied = 0;
        foreach (var mod in requestedItem.mods)
        {
            if (item.HasMod(mod)) modsSatisfied++;
        }

        return modsSatisfied == requestedItem.mods.Count;
    }
}
