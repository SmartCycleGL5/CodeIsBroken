using AYellowpaper.SerializedCollections;
using CodeIsBroken.Audio;
using FMODUnity;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;
using Random = System.Random;

namespace CodeIsBroken.Contract
{
    public class ContractManager : MonoBehaviour
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
        
        public static ContractManager instance;
        public static Contract ActiveContract { get; private set; }
        public static Action<Contract> OnNewContract;

        public static Settings activeSettings {get; private set;}
        [SerializedDictionary("level", "settings")]
        [SerializeField] SerializedDictionary<int, Settings> settings = new();

        public List<string> CompanyNames = new List<string>();


        public static VisualTreeAsset contractUI { get; private set;}
        public static VisualTreeAsset requestUI { get; private set;}
        public static VisualTreeAsset modifierUI { get; private set;}

        List<Contract> contractOptions = new();

        [Header("Audio")]
        [SerializeField] EventReference completedContract;
        private async void Start()
        {
            instance = this;
            UpdateContractComplexity(PlayerProgression.Level);
    
            PlayerProgression.onLevelUp += UpdateContractComplexity;

            if (contractUI == null)
            {
                contractUI = await Addressable.LoadAsset<VisualTreeAsset>("UI/Contract");
            }
            if (requestUI == null)
            {
                requestUI = await Addressable.LoadAsset<VisualTreeAsset>("UI/Contract/Request");
            }
            if (modifierUI == null)
            {
                modifierUI = await Addressable.LoadAsset<VisualTreeAsset>("UI/Contract/Modifier");
            }
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

            try
            {
                AudioManager.PlayOneShot(completedContract);
            }
            catch (Exception ex)
            {
            }
    
            GetContractOptions();
        }

        public static string GetCompanyName()
        {
            return instance.CompanyNames[UnityEngine.Random.Range(0, instance.CompanyNames.Count - 1)];
        }
    }
}


