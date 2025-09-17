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
        Debug.Log("got material");
        //if(GridBuilder.instance.LookUpCell(transform.position + transform.forward).TryGetComponent(out Conveyor conveyor))
        //{
        //    //add spawning on conveyor please :)))

        //    Instantiate(materialToSpawn.gameObject, conveyor.transform.position, conveyor.transform.rotation);
        //}
    }
}
