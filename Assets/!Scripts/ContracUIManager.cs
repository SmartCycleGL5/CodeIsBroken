using System.Threading.Tasks;
using CodeIsBroken.Product;
using Journal;
using UnityEngine;
using UnityEngine.UIElements;
using static UIManager;

public class ContracUIManager : MonoBehaviour
{
    static VisualElement contractHolder;
    static VisualElement selectText;

    static VisualTreeAsset contractUI;
    static VisualTreeAsset contractModifierUI;

    public static bool readyToTakeContacts;

    private async void Start()
    {
        contractHolder = canvas.Q<VisualElement>("ContractHolder");
        selectText = canvas.Q<Label>("Select");
        contractHolder.SetEnabled(false);
        readyToTakeContacts = false;
        selectText.style.opacity = 0;

        if (contractUI == null)
        {
            contractUI = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.Contract, AddressableToLoad.Object);
        }
        if(contractModifierUI == null)
        {
            contractModifierUI = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.ContractModifierElement, AddressableToLoad.Object);
        }
        Debug.Log("ContractSystem: "+contractHolder);
        readyToTakeContacts = true;
    }

    public static void DisplayContract(Contract[] contracts)
    {
        
        contractHolder.SetEnabled(true);
        selectText.style.opacity = 1;

        foreach (var contract in contracts)
        {
            CreateContract(contract);
        }
    }

    static void CreateContract(Contract contract)
    {
        Debug.Log("ContractSystem: Create");
        TemplateContainer contractContainer = contractUI.Instantiate();

        Button contractbutton = contractContainer.Q<Button>("Contract");
        contractbutton.Q<Label>("ContractName").text = contract.RequestedProduct.materials.ToString();
        contractbutton.Q<Label>("Amount").text = contract.amount.ToString() + " X";
        contractbutton.Q<Label>("XP").text = "Reward: " + contract.xpToGive + "xp";
        contractbutton.Q<VisualElement>("Icon").style.backgroundImage = new StyleBackground(ProductManager.GetProduct(contract.RequestedProduct).icon);

        ScrollView mods = contractbutton.Q<ScrollView>("Amount");

        foreach (var mod in contract.RequestedProduct.mods)
        {
            TemplateContainer modifierContainer = contractModifierUI.Instantiate();

            VisualElement modifier = modifierContainer.Q<VisualElement>("Modifier");
            modifier.Q<Label>("Name").text = mod.Name;
            modifier.Q<Label>("Description").text = mod.Description;

            mods.Add(modifier);
        }   

        contractHolder.Add(contractContainer);

        //such an ass solutuion, please try something else instead.
        contractbutton.clicked += () => { 
            ContractSystem.instance.SelectContract(contract);
            contractHolder.SetEnabled(false);
            selectText.style.opacity = 0;
            for (int i = contractContainer.childCount + 1; i >= 0; i--) { 
            
                contractHolder.RemoveAt(i);
            }
        };
    }
}
