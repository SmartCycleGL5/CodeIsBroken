using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Terminal
{
    using Language;

    public class ScriptManager : MonoBehaviour
    {
        public static Class UniversalClass { get { return instance._UniversalClass; } }
        public Class _UniversalClass;

        public static ScriptManager instance;

        public static bool isRunning { get; private set; }

        public List<MachineScript> machines = new();

        private void Awake()
        {
            instance = this;
        }

        [Button]
        public static void StartMachines()
        {
            if (isRunning) return;
            isRunning = true;

            foreach (var item in instance.machines)
            {
                item.Run();
            }
        }
        [Button]
        public static void StopMachines()
        {

        }

        public static void Re()
        {

            foreach (var item in instance.machines)
            {
                item.ClearMemory();
            }
        }

        public void AddMachine(MachineScript machine)
        {
            machines.Add(machine);
        }
        public void RemoveMachine(MachineScript machine)
        {
            machines.Remove(machine);
        }
    }
}
