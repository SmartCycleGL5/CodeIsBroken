
using System;
using CodeIsBroken.ProductSystem;
using UnityEngine;
using DG.Tweening;
using ScriptEditor.Console;
using Random = UnityEngine.Random;

namespace CodeIsBroken
{
    public class MaterialTube : Machine
    {
        
        Transform spawnLocation;
        //ProductSystem.Product materialToSpawn;
        int spawnRate;
        int tickCount;
        private GameObject lid;
        Sequence sequence;
        MaterialTubeSpawner materialTubeSpawner;


        private void Start()
        {
            Programmable machine = GetComponent<Programmable>();
            machine.AddMethodsAsIntegrated(typeof(MaterialTube));
            materialTubeSpawner = GetComponent<MaterialTubeSpawner>();
        }

        // Player controlled
        public void SpawnDelay(int delay)
        {
            materialTubeSpawner.SpawnDelay(delay);
        }
        public void SetMaterial(Material newMaterial)
        {
            if (!Enum.IsDefined(typeof(Material), newMaterial))
            {
                PlayerConsole.LogError("Invalid material type!", "MaterialTube");
                return;
            }
            materialTubeSpawner.SetMaterial(newMaterial);
        }

        public Material GetMaterial()
        {
            return materialTubeSpawner.currentMaterial;
        }

        private void OnDestroy()
        {
            
        }
    }

}