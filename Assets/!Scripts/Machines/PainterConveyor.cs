using System;
using CodeIsBroken.Product;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PainterConveyor : MonoBehaviour, IItemContainer
{
    Tweener moveTween;
    public Item item { get; set; }
    [SerializeField] ParticleSystem particle;
    
    void Start()
    {
        Tick.OnTick += TakeItem;
        Tick.OnEndingTick += Reset;
    }

    private void TakeItem()
    {
        // Output item
        GameObject outputCell = GridBuilder.instance.LookUpCell(transform.position+transform.forward);
        if(outputCell == null) return;
        if (outputCell.TryGetComponent(out Conveyor conveyorOut))
        {
            if (conveyorOut.item != null) return;
            conveyorOut.SetItem(item);
            RemoveItem();
        }

        //Take in item
        if (item != null) return;
        GameObject inputCell = GridBuilder.instance.LookUpCell(transform.position - transform.forward);
        if(inputCell == null) return;
        if(inputCell.TryGetComponent(out Conveyor conveyorIn))
        {
            if(conveyorIn.item == null) return;
            SetItem(conveyorIn.item);
            conveyorIn.RemoveItem();
        }

        //PainterMachine.item = item; we need a different solution lol
    }

    public void PaintEffect()
    {
            particle.Play();
    }

    private void Reset()
    {
        if (item != null)
        {
            Destroy(item.gameObject);
        }
    }

    public bool RemoveItem(out Item removedItem)
    {
        removedItem = null;
        if (item == null) return false;
        removedItem = item;
        item = null;
        if (moveTween != null)
        {
            moveTween.Kill();
        }
        return true;
    }

    public bool SetItem(Item item)
    {
        if (this.item != null) return false;
        this.item = item;
        moveTween = this.item.gameObject.transform.DOMove(transform.position+new Vector3(0,0.5f,0),0.3f);
        return true;
    }
    [DontIntegrate]
    public bool RemoveItem()
    {
        return RemoveItem(out Item item);
    }

    private void OnDestroy()
    { 
        Tick.OnTick -= TakeItem;
        Tick.OnEndingTick -= Reset;

        if (item == null) return;
        Debug.Log("Removed item from painter");
        Destroy(item.gameObject);
        RemoveItem();
        
    }
}