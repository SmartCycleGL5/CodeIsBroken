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


        public static bool CreateScript(GameObject gameObject, string name)
        {
            if(gameObject.TryGetComponent(out BaseMachine baseMachine))
            {
                return false;
            } else
            {
                baseMachine.Initialize(name);

                return true;
            }
        }

        [Button]
        public static void StartMachines()
        {
            if (isRunning) return;
            isRunning = true;

            Tick.StartTick();
        }
        [Button]
        public static void StopMachines()
        {
            if (!isRunning) return;
            Tick.StopTick();

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
