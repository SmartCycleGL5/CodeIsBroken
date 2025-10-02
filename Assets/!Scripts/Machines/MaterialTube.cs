using Unity.Mathematics;
using UnityEngine;

public class MaterialTube : Machine
{
    [SerializeField] Transform spawnLocation;
    Item materialToSpawn;
    int spawnRate;
    int tickCount;

    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));

        base.Initialize(initialClassName);
    }

    private void Start()
    {
        materialToSpawn = MaterialManager.Instance.items[Materials.Wood];
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
    public void ChangeMaterial(string material)
    {
        switch(material)
        {
            case "wood":
                {
                    materialToSpawn = MaterialManager.Instance.items[Materials.Wood];
                    break;
                }
            case "iron":
                {
                    materialToSpawn = MaterialManager.Instance.items[Materials.Iron];
                    break;
                }
        }
    }
    
    // Not player controlled
    
    [DontIntegrate]
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
        Item instObj = Instantiate(materialToSpawn.gameObject, conveyor.transform.position, conveyor.transform.rotation).GetComponent<Item>();
        conveyor.SetItem(instObj);
    }

    private void OnDisable()
    {
        Tick.OnTick -= GetMaterial;
    }
}
