using UnityEngine;

namespace CodeIsBroken
{
    public class Laser : Machine
    {
        [SerializeField] Transform cell;
    
        //protected override void Start() start is banned
        //{
        //    AddMethodsAsIntegrated(typeof(MaterialTube));
        //    base.Start();
        //}
    
        public bool Item()
        {
            GameObject cellObj = GridBuilder.instance.LookUpCell(cell.position);
            if(TryGetComponent(out Conveyor conveyor))
            {
                if(conveyor.item != null)
                {
                    return true;
                }
            }
            else
            {
                Debug.Log("[Laser] No conveyor found");
            }
            return false;
        }
    
    
    }

}
