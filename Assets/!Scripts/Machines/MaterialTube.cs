using Unity.Mathematics;
using UnityEngine;

public class MaterialTube : Machine
{
    [SerializeField] Transform spawnLocation;
    [SerializeField] Item materialToSpawn;
    int spawnRate;
    int tickCount;

    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));

        base.Initialize(initialClassName);
    }
    private void Start()
    {
        
    }

    // Player controlled
    public void SpawnRate(int spawnRate)
    {
        this.spawnRate = spawnRate;
        Tick.OnTick += GetMaterial;
    }
    
    // Not player controlled
    
    [DontIntegrate]
    public void GetMaterial()
    {
        tickCount++;
        if(tickCount < spawnRate) return;
        tickCount = 0;
        
        Debug.LogError("Reached max");

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

    private void OnDestroy()
    {
        Tick.OnTick -= GetMaterial;
    }
}
