using System;
using Unity.Mathematics;
using UnityEngine;

public class MaterialTube : Machine
{
    [SerializeField] Transform spawnLocation;
    Item materialToSpawn;
    int spawnRate;
    int tickCount;
    UserErrorLogger errorLogger;

    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));

        base.Initialize(initialClassName);
    }

    public override void Reset()
    {
        materialToSpawn = MaterialManager.Instance.Products[Materials.Wood];
        spawnRate = 0;
    }

    protected override void Start()
    {
        base.Start();
        materialToSpawn = MaterialManager.Instance.Products[Materials.Wood];
    }
    private void OnEnable()
    {
        errorLogger = GetComponent<UserErrorLogger>();
        Tick.OnTick += GetMaterial;
    }

    // Player controlled
    public void SpawnDelay(int delay)
    {
        this.spawnRate = delay;

    }
    public void SetMaterial(string material)
    {
        materialToSpawn = MaterialManager.Instance.Products[(Materials)Enum.Parse(typeof(Materials), material)];
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
            errorLogger.DisplayWarning("No conveyor found!");
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
    }

    private void OnDisable()
    {
        Tick.OnTick -= GetMaterial;
    }
}
