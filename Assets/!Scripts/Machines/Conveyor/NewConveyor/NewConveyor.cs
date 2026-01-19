using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CodeIsBroken.Machines;
using CodeIsBroken.ProductSystem;
using DG.Tweening;

public class NewConveyor : MonoBehaviour, IItemContainer
{
    public Product item { get; set; }
 
    List<NewConveyor> inputs = new List<NewConveyor>(); 
    List<NewConveyor> outputs = new List<NewConveyor>();
    List<Product> itemsHeld = new List<Product>(); 
  
    int maxItemsHeld;
    bool hasPulled = false;
    
    Tween moveTween;

    private void Start()
    {
        NewConveyorManager.instance.AddConveyor(this);
    }

    bool HasOutputConnection
    {
        get
        {
            return outputs.Count > 0;
        }
    }


    protected virtual bool CanPush()
    {        
       
        return itemsHeld.Count > 0;
    }
    

    public virtual bool CanReceive(Product item)
    {
        if(itemsHeld.Count == maxItemsHeld)
        {
            return false;
        }
        return true;
    }

    public void ResetState()
    {
        hasPulled = false;
    }   
       
    public void UpdateTick()
    {    

        if(!HasOutputConnection)
        {
            TryPull();
        }             
    }

    void TryPull()
    {    
        if(!hasPulled)
        {
            Pull();
        } 
    }

    void Pull()
    {
        hasPulled = true;

        for(int i = 0; i < inputs.Count; i ++)
        {
            inputs[i].TryPush(this);
        }
    }

    public void TryPush(NewConveyor to)
    {         
        if(CanPush())
        {
            Push(to);
        }  
        TryPull();
    }

    void Push(NewConveyor to)
    {      
        if(to.CanReceive(itemsHeld[0]))
        {
            to.InputReceived(itemsHeld[0]);
            itemsHeld.RemoveAt(0);
        }   
    }

    public void InputReceived(Product item)
    {
        itemsHeld.Add(item);
    }
    
    // IItemcontainer
    public bool RemoveItem(out Product removedItem)
    {   
        removedItem = null;
        if(item == null) return false;
        removedItem = item;
        item = null;
        if (moveTween != null)
        {
            moveTween.Kill();
        }
        return true;
    }
    

    public bool SetItem(Product item)
    {
        if (item == null) return false;
        if(this.item != null) return false;

        this.item = item;
        //Debug.Log(item.transform.position + " ");
        moveTween = this.item.gameObject.transform.DOMove(transform.position+new Vector3(0,0.5f,0),0.3f);
        return true;
    }
    
    public bool RemoveItem()
    {
        return RemoveItem(out Product item);
    }
}