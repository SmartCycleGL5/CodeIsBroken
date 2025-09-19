using UnityEngine;

public class MaterialTube : Machine
{
    [SerializeField] Transform spawnLocation;
    [SerializeField] Item materialToSpawn;

    public override void Initialize(string initialClassName)
    {
        AddMethodsAsIntegrated(typeof(MaterialTube));

        base.Initialize(initialClassName);
    }

    public void GetMaterial()
    {
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

        Debug.Log("[MaterialTube] got material");
        Instantiate(materialToSpawn.gameObject, conveyor.transform.position, conveyor.transform.rotation);
    }
}
