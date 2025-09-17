using UnityEngine;

public class MaterialTube : Machine
{
    [SerializeField] Transform spawnLocation;
    [SerializeField] Item materialToSpawn;

    protected override void Start()
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));

        base.Start();
    }

    public void GetMaterial()
    {
        Instantiate(materialToSpawn.gameObject, spawnLocation.position, spawnLocation.rotation);
    }
}
