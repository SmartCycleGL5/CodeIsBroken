using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Terminal
{
    using Language;
    using System;

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

        public static bool CreateScript(GameObject gameObject, string name)
        {
            if(gameObject.GetComponent<MachineScript>())
            {
                return false;
            } else
            {
                MachineScript script = gameObject.AddComponent<MachineScript>();
                script.machineCode =
                    "class " + name + "\n" +
                    "{\n" +
                    "\tvoid Start()\n" +
                    "\t{\n" +
                    "\n\t\t" +
                    "\n\t\t" +
                    "\n\t\t" +
                    "\t}\n" +
                    "}";

                return true;
            }
            return false;
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
            foreach (var item in instance.machines)
            {
                item.Stop();
            }

            isRunning = false;
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
