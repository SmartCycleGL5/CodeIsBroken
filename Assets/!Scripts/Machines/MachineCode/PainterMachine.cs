
using UnityEngine;

namespace CodeIsBroken
{
    public class PainterMachine : Machine
    {
        Item item;
        private PainterConveyor painterConveyor;
        Renderer toColor { get { return item.artRenderer; } }

        void Start()
        {
            BaseMachine machine = GetComponent<BaseMachine>();
            machine.AddMethodsAsIntegrated(typeof(PainterMachine));
            
            //painterConveyor = GetComponent<PainterConveyor>();
        }
        public void Paint(string PrimaryColor)
        {/*
            item = painterConveyor.item;
            Metrics.instance.UseElectricity(1);
            Debug.Log("Set color to: " + PrimaryColor);
            item = painterConveyor.item;
            if (item == null) return;
    
            switch (PrimaryColor)
            {
                case "Red":
                    item.definition.Modify(new Modification.Color(new Color(1, 0, 0)));
                    return;
                case "Blue":
                    item.definition.Modify(new Modification.Color(new Color(0, 0, 1)));
                    return;
                case "Green":
                    item.definition.Modify(new Modification.Color(new Color(0, 1, 0)));
                    return;
                default:
                    return;
    
            }*/
        }
    
        //private void OnDestroy()
        //{
        //    if (item == null) return;
        //    Destroy(item);
        //}
    
    }

}
