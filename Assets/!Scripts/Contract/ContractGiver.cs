using System;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace CodeIsBroken.Contract
{
    public class ContractGiver : MonoBehaviour
    {
        public static ContractGiver instance;
        public static Contract ActiveContract { get; private set; }
        public static Action<Contract> OnNewContract;
    
        [Header("Contract Settings"), MinMaxSlider(0, 10)]
        public Vector2Int additionalModifications;
    
    
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
                        additionalModifications.y++;
                        break;
                    }
                case 4:
                    {
                        additionalModifications.y++;
                        additionalModifications.x++;
                        break;
                    }
                case 5:
                    {
                        break;
                    }
            }
        }
        public void SelectContract(Contract toSelect)
        {
            toSelect.onFinished += instance.FinishedContract;
    
            ActiveContract = toSelect;
            
            OnNewContract?.Invoke(toSelect);
        }
    
        public static Contract NewContract()
        {
            Contract contract = new Contract(
                "cool contract", 
                Mathf.RoundToInt(UnityEngine.Random.Range(instance.additionalModifications.x, instance.additionalModifications.y + 1))
                );
    
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
            List<Contract> contracts = new();
    
            for (int i = 0; i < 3; i++)
            {
                contracts.Add(NewContract());
            }
    
            while(!ContracUIManager.readyToTakeContacts) await Task.Delay(100);
    
            ContracUIManager.DisplayContract(contracts.ToArray());
        }
    }
}


