
using System;
using CodeIsBroken;
using CodeIsBroken.ProductSystem;
using UnityEngine;
using DG.Tweening;
using ScriptEditor.Console;
using Console = CodeIsBroken.IDE.Console;
using Material = CodeIsBroken.Material;
using Product = CodeIsBroken.Product;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(-100)]
public class MaterialTubeSpawner : MonoBehaviour
{
    public Material currentMaterial;
    Transform spawnLocation;
    CodeIsBroken.ProductSystem.Product materialToSpawn;
    int spawnRate;
    int tickCount;
    private GameObject lid;
    Sequence sequence;
    

    private void Reset()
    {
        SetMaterial(Material.wood);
        spawnRate = 0;
    }

    private void Start()
    {
        Tick.OnLateTick += GetMaterial;
        
        //Set all references
        ReferenceHolder referenceHolder = GetComponent<ReferenceHolder>();
        spawnLocation = referenceHolder.GetReference("spawnLocation").transform;
        lid = referenceHolder.GetReference("lid").gameObject;
        

        sequence.Append(lid.transform.DOLocalRotate(new Vector3(-130, 0, 0), 0.2f).OnComplete(CloseLid));
        sequence.Append(lid.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutBounce).SetDelay(0.2f));

        ProductManager.foundProducts += () => { SetMaterial(Material.wood); };

        Tick.OnStartingTick += Reset;
    }

    // Player controlled
    public void SpawnDelay(int delay)
    {
        this.spawnRate = delay;

    }
    public void SetMaterial(Material material)
    {
        currentMaterial = material;
        materialToSpawn = ProductManager.GetProduct((Product)material);
    }
    
    // Not player controlled
    
    private void GetMaterial()
    {
        if(materialToSpawn == null) return;
        tickCount++;
        if(tickCount < spawnRate) return;
        tickCount = 0;
        
        //Debug.LogError("Reached max");

        //GameObject cell = GridBuilder.instance.LookUpCell(transform.position + transform.forward);
        

        // if (cell == null)
        // {
        //     Debug.Log("[MaterialTube] Nothing in adjacent cell");
        //     return;
        // }
        //
        // if (!cell.TryGetComponent(out Conveyor conveyor))
        // {
        //     Debug.Log("[MaterialTube] Adjacent cell not conveyor");
        //     return;
        // }
        Conveyor conveyor = GetComponent<Conveyor>();
        if(conveyor.item != null)return;
        Debug.Log("[MaterialTube] got material");
        CodeIsBroken.ProductSystem.Product instObj = Instantiate(materialToSpawn.gameObject, conveyor.transform.position+new Vector3(0,0,0), conveyor.transform.rotation).GetComponent<CodeIsBroken.ProductSystem.Product>();
        instObj.gameObject.transform.Rotate(new Vector3(0, UnityEngine.Random.Range(0, 359), 0));
        conveyor.SetItem(instObj);
        lid.transform.DOLocalRotate(new Vector3(-130, 0, 0), 0.2f).OnComplete(CloseLid);

    }

    void CloseLid()
    {
        Debug.Log("CloseLid");
        lid.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.4f).SetEase(Ease.OutBounce).SetDelay(0.2f);
    }

    private void OnDestroy()
    {
        Tick.OnTick -= GetMaterial;
        Tick.OnLateTick -= Reset;
    }


}