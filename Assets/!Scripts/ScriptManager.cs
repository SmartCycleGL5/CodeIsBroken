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

        public List<BaseMachine> machines = new();

        private void Awake()
        {
            instance = this;
        }

        public static void CreateNewTerminal(BaseMachine machineScript)
        {
            Terminal newTerminal = UIManager.Instance.gameObject.AddComponent<Terminal>();
            newTerminal.SelectMachine(machineScript);
        }


        public static bool CreateScript(GameObject gameObject, string name)
        {
            if(gameObject.GetComponent<BaseMachine>())
            {
                return false;
            } else
            {
                BaseMachine script = gameObject.AddComponent<BaseMachine>();
                script.machineCode.CreateScript(name);

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
