using NaughtyAttributes;
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

    [Header("Contract Settings")]
    public int amountOfModifications = 2;
    [MinMaxSlider(1, 10)]
    [SerializeField] Vector2Int complexity = new Vector2Int(1, 1);


    private void Start()
    {
        NewContract();
        instance = this;

        PlayerProgression.onLevelUp += UpdateContractComplexity;
    }

    private void UpdateContractComplexity(int lvl)
    {
        if(lvl == 3)
        {
            complexity.y++;
        }
    }

    private void Update()
    {
        if (ActiveContract == null) return;
        amoundDisplay.text = "X" + ActiveContract.amount;
    }

    void CreateDisplayItem()
    {
        if (displayItem != null) Destroy(displayItem.gameObject);

        bool isProduct = UnityEngine.Random.Range(0, 2) == 0;
        Item toCreate = MaterialManager.Instance.Products[ActiveContract.requestedItem.materials];

        displayItem = Instantiate(toCreate, displayPos);

        displayItem.destroyOnPause = false;
        displayItem.definition = ActiveContract.requestedItem;
    }

    public void NewContract()
    {
        Contract contract = new Contract("cool contract", amountOfModifications, Mathf.RoundToInt(UnityEngine.Random.Range(complexity.x, complexity.y + 1)));
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



    public Contract(string name, int amountOfMods, int complexity)
    {
        contractName = name;

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

        amount = Mathf.RoundToInt(UnityEngine.Random.Range(PlayerProgression.Level * 5, (PlayerProgression.Level * 5) * 1.5f));


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
        PlayerProgression.GiveXP(10);
        onFinished?.Invoke(this);
    }

    public bool SatisfiesContract(Item item)
    {
        return item.definition.Equals(requestedItem);
    }
}
