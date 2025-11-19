using System;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace CodeIsBroken.Contract
{
    public class ContractGiver : MonoBehaviour
    {
        [Serializable]
        public class Settings
        {
            [MinMaxSlider(0, 10)]
            public Vector2Int additionalModifications;
            
            [Space]
            [InfoBox("Max 3")]
            public PredeterminedContract[] firstContracts;
        }
        
        public static ContractGiver instance;
        public static Contract ActiveContract { get; private set; }
        public static Action<Contract> OnNewContract;

        public static Settings activeSettings {get; private set;}
        [SerializedDictionary("level", "settings")]
        [SerializeField] SerializedDictionary<int, Settings> settings = new();
        
        List<Contract> contractOptions = new();
        private void Start()
        {
            instance = this;
            UpdateContractComplexity(PlayerProgression.Level);
    
            PlayerProgression.onLevelUp += UpdateContractComplexity;
        }
    
        private void UpdateContractComplexity(int lvl)
        {
            if (settings.ContainsKey(lvl))
            {
                activeSettings = settings[lvl];
                
                Debug.Log(activeSettings.firstContracts.Length);


                if (activeSettings.firstContracts.Length > 0)
                {
                    contractOptions.Clear();
                    
                    foreach (var contract in activeSettings.firstContracts)
                    {
                        contractOptions.Add(contract.GetContract());
                    }   
                }
            }
            switch(lvl)
            {
                case 2:
                    {
                        GetContractOptions();
                        break;
                    }
            }
        }
        
        async void GetContractOptions()
        {

            for (int i = contractOptions.Count; i < 3; i++)
            {
                contractOptions.Add(Contract.New());
            }
    
            while(!ContracUIManager.readyToTakeContacts) await Task.Delay(100);
    
            ContracUIManager.DisplayContract(contractOptions.ToArray());
            
            contractOptions.Clear();
        }
        
        public void SelectContract(Contract toSelect)
        {
            toSelect.onFinished += instance.FinishedContract;
    
            ActiveContract = toSelect;
            
            OnNewContract?.Invoke(toSelect);
        }
        
        void FinishedContract(Contract contract)
        {
            if (contract != ActiveContract) return;
    
            ActiveContract.onFinished -= instance.FinishedContract;
            ActiveContract = null;
    
            GetContractOptions();
        }
    }
}


