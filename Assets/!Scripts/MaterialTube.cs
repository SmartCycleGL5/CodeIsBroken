using UnityEngine;

public class MaterialTube : Machine
{
    [SerializeField] Transform spawnLocation;
    [SerializeField] Item materialToSpawn;

    protected override void Start()
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));

        spawnLocation = transform;

        base.Start();
    }

    public void GetMaterial()
    {
        //Debug.Log(gameObject);      
        Debug.Log(GridBuilder.instance);
        GameObject ahead = GridBuilder.instance.LookUpCell(spawnLocation.position + spawnLocation.forward);
        if (ahead == null) return;
        if (ahead.TryGetComponent(out Conveyor conveyor))
        {
            Debug.Log("GetMaterial");
            //add spawning on conveyor please :)))

            Instantiate(materialToSpawn.gameObject, conveyor.transform.position, conveyor.transform.rotation);
        }
    }
}
