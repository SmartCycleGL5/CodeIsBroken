
using CodeIsBroken.ProductSystem;
using UnityEngine;

namespace CodeIsBroken
{
    public class Painter : Machine
    {
        ProductSystem.Product item;
        private PainterConveyor painterConveyor;
        Renderer toColor { get { return item.artRenderer; } }

        void Start()
        {
            Programmable machine = GetComponent<Programmable>();
            machine.AddMethodsAsIntegrated(typeof(Painter));
            
            painterConveyor = GetComponent<PainterConveyor>();

            Tick.OnTick += UpdateItem;
        }

        private void UpdateItem()
        {
            item = painterConveyor.item;
            Debug.Log(item);
        }
        
        public bool HasItem()
        {
            return item != null;
        }
        public void Paint(Color color)
        {
            Debug.Log("[Painter] im alive");
            Metrics.instance.UseElectricity(1);
            painterConveyor.PaintEffect();
            Debug.Log("Set color to: " + color);
            if (item == null) return;
            
            item.Modify(ProductSystem.Modifications.Color.New(new UnityEngine.Color(color.r, color.g, color.b, color.a)));
        }
    
        void OnDestroy()
        {
            Tick.OnTick -= UpdateItem;
            if (item == null) return;
            item = null;
        }
    
    }

}
