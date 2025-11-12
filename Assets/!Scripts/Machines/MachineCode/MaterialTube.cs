using System;
using ScriptEditor.Console;
using UnityEngine;
using DG.Tweening;

namespace Machines
{
    public class MaterialTube : Machine
    {
        Transform spawnLocation;
        Item materialToSpawn;
        int spawnRate;
        int tickCount;
        private GameObject lid;
        Sequence sequence;
        

        public override void Reset()
        {
            materialToSpawn = MaterialManager.Instance.Products[Materials.Wood];
            spawnRate = 0;
        }

        private void Awake()
        {
            materialToSpawn = MaterialManager.Instance.Products[Materials.Wood];
            
            //Set all references
            ReferenceHolder referenceHolder = GetComponent<ReferenceHolder>();
            spawnLocation = referenceHolder.GetReference("spawnLocation").transform;
            lid = referenceHolder.GetReference("lid").gameObject;
            

            sequence.Append(lid.transform.DOLocalRotate(new Vector3(-130, 0, 0), 0.2f).OnComplete(CloseLid));
            sequence.Append(lid.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutBounce).SetDelay(0.2f));
        }
        private void OnEnable()
        {
            Tick.OnTick += GetMaterial;
        }

        // Player controlled
        public void SpawnDelay(int delay)
        {
            this.spawnRate = delay;

        }
        public void SetMaterial(string material)
        {
            Debug.Log(material);
            materialToSpawn = MaterialManager.Instance.Products[(Materials)Enum.Parse(typeof(Materials), material)];
            Debug.Log(materialToSpawn);
        }
        
        // Not player controlled
        
        public void GetMaterial()
        {
            tickCount++;
            if(tickCount < spawnRate) return;
            tickCount = 0;
            
            //Debug.LogError("Reached max");

            GameObject cell = GridBuilder.instance.LookUpCell(transform.position + transform.forward);

            if (cell == null)
            {
                Debug.Log("[MaterialTube] Nothing in adjacent cell");
                return;
            }

            if (!cell.TryGetComponent(out Conveyor conveyor))
            {
                Debug.Log("[MaterialTube] Adjacent cell not conveyor");
                return;
            }
            if(conveyor.item != null)return;
            Debug.Log("[MaterialTube] got material");
            Item instObj = Instantiate(materialToSpawn.gameObject, conveyor.transform.position+new Vector3(0,1,0), conveyor.transform.rotation).GetComponent<Item>();
            conveyor.SetItem(instObj);
            lid.transform.DOLocalRotate(new Vector3(-130, 0, 0), 0.2f).OnComplete(CloseLid);

        }

        void CloseLid()
        {
            Debug.Log("CloseLid");
            lid.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutBounce).SetDelay(0.2f);
        }

        private void OnDisable()
        {
            Tick.OnTick -= GetMaterial;
        }
    }

}
