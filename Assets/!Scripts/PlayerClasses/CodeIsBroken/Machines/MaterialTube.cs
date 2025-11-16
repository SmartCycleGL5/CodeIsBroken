
using CodeIsBroken.Product;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace CodeIsBroken
{
    public class MaterialTube : Machine
    {
        
        Transform spawnLocation;
        Item materialToSpawn;
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
        public void SetMaterial(ProductDefinition material)
        {
            materialTubeSpawner.SetMaterial(material);
        }
        
        

        private void OnDestroy()
        {
            
        }
    }

}