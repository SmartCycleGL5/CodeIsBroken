using Coding;
using Journal;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeIsBroken.Item;
using TMPro;
using UnityEngine;

public class ContractSystem : MonoBehaviour
{
    public static ContractSystem instance;
    public static Contract ActiveContract;

    [Header("Contract Settings")]
    public int amountOfModifications = 2;
    [MinMaxSlider(1, 10)]
    [SerializeField] Vector2Int complexity = new Vector2Int(1, 1);


    private void Start()
    {
        instance = this;

        PlayerProgression.onLevelUp += UpdateContractComplexity;
    }

    private void UpdateContractComplexity(int lvl)
    {
        switch(lvl)
        {
            case 2:
                {
                    GetContractOptions();
                    break;
                }
            case 3:
                {
                    complexity.y++;
                    break;
                }
        }
    }
    public void SelectContract(Contract toSelect)
    {
        toSelect.onFinished += instance.FinishedContract;

        ActiveContract = toSelect;
        
        JournalManager.instance.Contract(toSelect);
    }

    public static Contract NewContract()
    {
        Contract contract = new Contract(
            "cool contract", 
            instance.amountOfModifications, 
            Mathf.RoundToInt(UnityEngine.Random.Range(instance.complexity.x, instance.complexity.y + 1)));

        return contract;
    }
    void FinishedContract(Contract contract)
    {
        if (contract != ActiveContract) return;

        ActiveContract.onFinished -= instance.FinishedContract;
        ActiveContract = null;

        GetContractOptions();
    }

    async void GetContractOptions()
    {
        ScriptManager.StopMachines();
        List<Contract> contracts = new();

        for (int i = 0; i < 3; i++)
        {
            contracts.Add(NewContract());
        }

        while(!ContracUIManager.readyToTakeContacts) await Task.Delay(100);

        ContracUIManager.DisplayContract(contracts.ToArray());
    }
}

public class Contract
{
    public string contractName;
    public ItemDefinition requestedItem;
    public int amount;

    public Action<Contract> onFinished;

    public int xpToGive;

    readonly string[] names = new string[]
    {
        "Wood",
        "Stone",
        "Iron",
        "Wood Carvers",
        "Stone Carvers"
    };

    public Contract(string name, int amountOfMods, int complexity)
    {
        contractName = names[UnityEngine.Random.Range(0, names.Length - 1)];

        List<Modification> mods = new List<Modification>();

        for (int i = 0; i < amountOfMods; i++)
        {
            Modification newMod = Modification.RandomModification();


            if (AlreadyHasMod(newMod))
            {
                Debug.Log("already has mod");
                continue;
            }

            Debug.Log(newMod);
            mods.Add(newMod);
        }

        requestedItem = new(MaterialManager.GetRandomProduct(complexity), mods);

        amount = Mathf.RoundToInt(UnityEngine.Random.Range(PlayerProgression.Level * 5, (PlayerProgression.Level * 5) * 2));

        xpToGive = amount * 6;


        bool AlreadyHasMod(Modification newMod)
        {
            foreach (var mod in mods)
            {
                if (mod.Compare(newMod)) return true;
            }

            return false;
        }
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
        PlayerProgression.GiveXP(xpToGive);
        onFinished?.Invoke(this);
    }

    public bool SatisfiesContract(Item item)
    {
        return item.definition.Equals(requestedItem);
    }
}
