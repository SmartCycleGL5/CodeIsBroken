using CodeIsBroken.Product;
using UnityEngine;
using DG.Tweening;


namespace CodeIsBroken
{
    public class Crane : Machine, IItemContainer
    {
        private Transform piviot;
        private Transform arm;
        private Transform chain;
        private Transform grabLocation;
        private Transform holdLocation;
    
        Tweener rotationTween;
        private bool pickUp;
        private bool drop;
        public Product.Item item { get; set; }

        private void Start()
        {
            Programmable machine = GetComponent<Programmable>();
            machine.AddMethodsAsIntegrated(typeof(Crane));
            
            ReferenceHolder referenceHolder = GetComponent<ReferenceHolder>();
            piviot = referenceHolder.GetReference("craneBase").transform;
            holdLocation = referenceHolder.GetReference("holdLocation").transform;
            grabLocation = referenceHolder.GetReference("grabLocation").transform;
            Debug.Log("Crane: "+piviot);
        }

        public bool HasItem()
        {
            return item != null;
        }
        public void Rotate(int degrees)
        {
            Metrics.instance.UseElectricity((int)degrees/90);
    
            if (rotationTween != null) return;
            rotationTween = piviot.DOLocalRotate(new Vector3(0,0,piviot.localEulerAngles.z+degrees), 0.5f, RotateMode.FastBeyond360).OnComplete(() =>
            {
                if(pickUp || drop) GrabLoseItem();
                rotationTween = null;
            });
        }
    
        public void PickUp()
        {
            if (item != null) return;
            if (rotationTween != null)
            {
                pickUp = true;
            }
            else
            {
                pickUp = true;
                GrabLoseItem();
            }
        }

        public void Drop()
        {
            if (item == null) return;
            if (rotationTween != null)
            {
                drop = true;
            }
            else
            {
                drop = true;
                GrabLoseItem();
            }
        }
        
        private void GrabLoseItem()
        {
            //Metrics.instance.UseElectricity(1);
            GameObject cell = GridBuilder.instance.LookUpCell(grabLocation.position);
            Debug.Log("Craneee, "+cell);
            if (cell == null)
            {
                Debug.Log("[Crane] Nothing in cell");
                return;
            }
    
            if (!cell.TryGetComponent(out Conveyor conveyor))
            {
                Debug.Log("[Crane] Cell not conveyor");
                return;
            }
    
            if (item == null)
            {
                if (conveyor.item == null)
                {
                    Debug.Log("[Crane] No Item on conveyor");
                    return;
                }
    
                if (pickUp)
                {
                    SetItem(conveyor.item);
                    Debug.Log("[Crane] Grab");
                    conveyor.RemoveItem();
                    pickUp = false;
                }
            }
            else if (drop && conveyor.item == null)
            {
                conveyor.SetItem(item);
                RemoveItem();
                drop = false;
            }
        }
        
        
        
        public bool SetItem(Product.Item item)
        {
    
            if (this.item != null) return false;
    
            this.item = item;
            this.item.transform.parent = holdLocation;
            this.item.transform.position = holdLocation.position;
            return true;
        }
        public bool RemoveItem(out Product.Item removedItem)
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
            return RemoveItem(out Product.Item item);
        }
    
    }

}
