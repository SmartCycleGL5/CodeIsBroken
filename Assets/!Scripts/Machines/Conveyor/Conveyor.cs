using DG.Tweening;
using System;
using System.Collections.Generic;
using CodeIsBroken.ProductSystem;
using UnityEngine;


public class Conveyor : MonoBehaviour, IItemContainer
{
    // Conveyor to send item to next
    
    [SerializeField] private Transform output;
    
    public Conveyor nextConveyor;
    public List<Conveyor> recieveFrom = new();

    [SerializeField] List<Transform> inputs;
    [SerializeField] private Renderer renderer;
    
    private Material material;
    private Tween moveTween;
    
    
    public Product item { get; set; }


    //Vector3 itemPosition { get { return transform.position + new Vector3(0, 1, 0);  } }

    void Start()
    {
        if (renderer != null)
        {
            material = renderer.material;
        }
        
        if (GameManager.isRunning)
        {
            StartAnim();
        }
        else
        {
            StopAnim();
        }
        //Checks for other conveyors and update the conveyors that found.
        UpdateConveyor();
        ConveyorManager.instance.UpdateCells(transform.position+transform.forward);
        foreach(var pos in inputs)
        {
            ConveyorManager.instance.UpdateCells(pos.position);
        }
        UpdateConveyor();
        Tick.OnLateTick += MoveOnTick;
        if (renderer != null)
        {
            
        }
        
    }

    void StartAnim()
    {
        //material.SetVector("_direction", new Vector2(0,0.6f));
    }

    void StopAnim()
    {
        //material.SetVector("_direction", new Vector2(0,0));
    }

    public void UpdateConveyor()
    {
        recieveFrom.Clear();
        ConveyorManager cm = ConveyorManager.instance;
        nextConveyor = cm.GetConveyor(output.position);
        //if (inputs.Count <= 0) return;
        foreach (var pos in inputs)
        {
            Conveyor conveyor = cm.GetConveyor(pos.position);
            if(conveyor == null) continue;
            if(conveyor.nextConveyor != this) continue;
            Debug.LogError("Found compatible conveyor");
            recieveFrom.Add(conveyor);
        }

    }

    void MoveOnTick()
    {
        // Checks if last in line
        if (nextConveyor == null && recieveFrom.Count > 0)
        {
            Debug.Log("LastInLine: "+nextConveyor);
            SendItem();
        }
    }

    public void SendItem()
    {
        if(recieveFrom != null)
        {
            foreach (var conveyor in recieveFrom)
            {
                if (conveyor == null) return;
                if (item == null)
                {
                    SetItem(conveyor.item);
                    conveyor.RemoveItem();
                }
                conveyor.SendItem();
            }
        }
    }
    

    private void OnDestroy()
    {
        Tick.OnLateTick -= MoveOnTick;
        if (item != null)
        {
            Destroy(item.gameObject);
        }
    }

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