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
    }

    void CreateDisplayItem()
    {
        if (displayItem != null) Destroy(displayItem.gameObject);

        Debug.Log(MaterialManager.Instance);

        displayItem = Instantiate(MaterialManager.Instance.items[Materials.Wood], displayPos);
        displayItem.destroyOnPause = false;
        displayItem.definition = ActiveContract.requestedItem;
    }

    public void NewContract()
    {
        Contract contract = new Contract("cool contract");
        contract.onFinished += FinishedContract;

        ActiveContract = contract;

        CreateDisplayItem();
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

    public ItemDefinition requestedItem;

    public int amount;

    public Action<Contract> onFinished;

    public Contract(string name)
    {
        contractName = name;

        List<Modification> mods = new List<Modification>();

        for (int i = 0; i < 2; i++)
        {
            mods.Add(Modification.RandomModification());
        }

        requestedItem = new(Materials.Wood, mods);

        amount = UnityEngine.Random.Range(10, PlayerProgression.Level * 10 + 10);
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
        return item.definition.Equals(requestedItem);
    }
}
