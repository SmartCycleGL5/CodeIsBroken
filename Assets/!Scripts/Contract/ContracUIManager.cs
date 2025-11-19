using UnityEngine;
using UnityEngine.UIElements;
using Utility;
using static CodeIsBroken.UI.UIManager;

namespace CodeIsBroken.Contract
{
    public class ContracUIManager : MonoBehaviour
    {
        static VisualElement contractHolder;
        static VisualElement selectText;
    
        public static bool readyToTakeContacts;
    
        private void Start()
        {
            contractHolder = canvas.Q<VisualElement>("ContractHolder");
            selectText = canvas.Q<Label>("Select");
            readyToTakeContacts = false;
            contractHolder.style.visibility = Visibility.Hidden;
            selectText.style.visibility = Visibility.Hidden;

            Debug.Log("ContractSystem: "+contractHolder);
            readyToTakeContacts = true;
        }
    
        public static void DisplayContract(Contract[] contracts)
        {
            contractHolder.style.visibility = Visibility.Visible;
            selectText.style.visibility = Visibility.Visible;

            foreach (var contract in contracts)
            {
                CreateContract(contract);
            }
        }
    
        static void CreateContract(Contract contract)
        {
            Debug.Log("ContractSystem: Create");

            TemplateContainer contractUI = contract.GetUI();
            contractHolder.Add(contractUI);

            contractUI.Q<Button>("Select").clicked += () => { 
                ContractManager.instance.SelectContract(contract);

                for (int i = contractHolder.childCount -1; i >= 0; i--) { 
                
                    contractHolder.RemoveAt(i);
                }

                contractHolder.style.visibility = Visibility.Hidden;
                selectText.style.visibility = Visibility.Hidden;
            };
        }
    }
}

