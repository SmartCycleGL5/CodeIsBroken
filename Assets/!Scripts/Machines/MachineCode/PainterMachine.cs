
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
            
            painterConveyor = GetComponent<PainterConveyor>();

            Tick.OnTick += UpdateItem;
        }

        private void UpdateItem()
        {
            item = painterConveyor.item;
        }
        
        public bool HasItem()
        {
            return item != null;
        }
        public void Paint(string PrimaryColor)
        {
            Metrics.instance.UseElectricity(1);
            Debug.Log("Set color to: " + PrimaryColor);
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
    
            }
        }
    
        void OnDestroy()
        {
            if (item == null) return;
            Destroy(item);
        }
    
    }

}
