using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Coding
{
    using Language;
    using System;

    public class ScriptManager : MonoBehaviour
    {
        public static ScriptManager instance;

        public static bool isRunning { get; private set; }

        public List<BaseMachine> machines = new();

        private void Awake()
        {
            instance = this;
        }


        public static BaseMachine CreateScript(GameObject gameObject, string name)
        {
            if(gameObject.TryGetComponent(out BaseMachine baseMachine))
            {
                baseMachine.Initialize(name);
                return baseMachine;
            }

            return null;
        }

        [Button]
        public static void StartMachines()
        {
            if (isRunning) return;
            isRunning = true;

            Debug.Log("[ScriptManager] Starting");

            Tick.StartTick();
        }
        [Button]
        public static void StopMachines()
        {
            if (!isRunning) return;
            Tick.StopTick();

            Debug.Log("[ScriptManager] Ending");

            isRunning = false;
        }

        public static void Re()
        {

            foreach (var item in instance.machines)
            {
                item.ClearMemory();
            }
        }

        public void AddMachine(BaseMachine machine)
        {
            machines.Add(machine);
        }
        public void RemoveMachine(BaseMachine machine)
        {
            machines.Remove(machine);
        }
    }
}
