using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static UIManager;

public class ContracUIManager : MonoBehaviour
{
    static VisualElement contractHolder;

    static VisualTreeAsset contractUI;
    static VisualTreeAsset contractModifierUI;

    private async void Start()
    {
        contractHolder = canvas.Q<VisualElement>("ContractHolder");

        if (contractUI == null)
        {
            contractUI = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.Contract, AddressableToLoad.GameObject);
        }
        if(contractModifierUI == null)
        {
            contractModifierUI = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.ContractModifierElement, AddressableToLoad.GameObject);
        }
    }

    public static async void DisplayContract(Contract[] contracts)
    {
        foreach (var contract in contracts)
        {
            await CreateContract(contract);
        }
    }

    async static Task CreateContract(Contract contract)
    {
        while(contractUI == null)
        {
            await Task.Delay(10);
        }
        while (contractModifierUI == null)
        {
            await Task.Delay(10);
        }
        TemplateContainer contractContainer = contractUI.Instantiate();

        Button contractbutton = contractContainer.Q<Button>("Contract");
        contractbutton.Q<Label>("ContractName").text = contract.contractName;
        contractbutton.Q<Label>("Amount").text = contract.amount.ToString() + " X";
        contractbutton.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(MaterialManager.Instance.Products[contract.requestedItem.materials].icon);

        ScrollView mods = contractbutton.Q<ScrollView>("Amount");

        foreach (var mod in contract.requestedItem.mods)
        {
            TemplateContainer modifierContainer = contractModifierUI.Instantiate();

            VisualElement modifier = modifierContainer.Q<VisualElement>("Modifier");
            modifier.Q<Label>("Name").text = mod.Name;
            modifier.Q<Label>("Description").text = mod.Description;

            mods.Add(modifier);
        }

        contractHolder.Add(contractContainer);

        //such a ass solutuion, please try something else instead.
        contractbutton.clicked += () => { 
            ContractSystem.SelectContract(contract); 
            for (int i = contractContainer.childCount + 1; i >= 0; i--) { 
            
                contractHolder.RemoveAt(i);
            } 
        };
    }
}
