using System;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;


namespace CodeIsBroken
{
    public class CraneMachine : Machine, IItemContainer
    {
        private Transform piviot;
        private Transform arm;
        private Transform chain;
        private Transform grabLocation;
        private Transform holdLocation;
    
        Tweener rotationTween;
        private bool pickUp;
        private bool drop;
        public Item item { get; set; }

        private void Start()
        {
            ReferenceHolder referenceHolder = GetComponent<ReferenceHolder>();
            piviot = referenceHolder.GetReference("craneBase").transform;
            Debug.Log("Crane: "+piviot);
        }

        public void Rotate(int degrees)
        {
            Metrics.instance.UseElectricity((int)degrees/90);
    
            if (rotationTween != null) return;
            rotationTween = piviot.DORotate(new Vector3(0, piviot.rotation.eulerAngles.y+degrees, 0), 0.5f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                rotationTween = null;
                if(pickUp) GrabLoseItem();
                pickUp = false;
            });
        }
    
        public void PickUp()
        {
            if (rotationTween != null)
            {
                pickUp = true;
            }
            else
            {
                GrabLoseItem();
            }
        }

        public void Drop()
        {
            if (rotationTween != null)
            {
                drop = true;
            }
            else
            {
                GrabLoseItem();
            }
        }
        
        private void GrabLoseItem()
        {
            //Metrics.instance.UseElectricity(1);
            GameObject cell = GridBuilder.instance.LookUpCell(grabLocation.position);
    
            if (cell == null)
            {
                pickUp = false;
                drop = false;
                Debug.Log("[Crane] Nothing in cell");
                return;
            }
    
            if (!cell.TryGetComponent(out Conveyor conveyor))
            {
                pickUp = false;
                drop = false;
                Debug.Log("[Crane] Cell not conveyor");
                return;
            }
    
            if (item == null)
            {
                if (conveyor.item == null)
                {
                    Debug.Log("[Crane] No Item on conveyor");
                    pickUp = false;
                    drop = false;
                    return;
                }
    
                if (pickUp && SetItem(conveyor.item))
                {
                    Debug.Log("[Crane] Grab");
                    conveyor.RemoveItem();
                    pickUp = false;
                }
            }
            else if (drop && conveyor.SetItem(item))
            {
                RemoveItem();
                drop = false;
            }
        }
        public bool SetItem(Item item)
        {
    
            if (this.item != null) return false;
    
            this.item = item;
            this.item.transform.parent = holdLocation;
            this.item.transform.position = holdLocation.position;
            return true;
        }
        public bool RemoveItem(out Item removedItem)
        {
            removedItem = null;
            if (item == null) return false;
            removedItem = item;
            item.transform.parent = null;
            item = null;
            return true;
        }
        public bool RemoveItem()
        {
            return RemoveItem(out Item item);
        }
    
    }

}
