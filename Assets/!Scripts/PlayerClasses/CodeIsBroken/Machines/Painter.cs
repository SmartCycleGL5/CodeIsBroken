
using CodeIsBroken.Product;
using UnityEngine;

namespace CodeIsBroken
{
    public class Painter : Machine
    {
        Product.Item item;
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
            
            item.definition.Modify(Product.Modifications.Color.New(new UnityEngine.Color(color.r, color.g, color.b, color.a)));
        }
    
        void OnDestroy()
        {
            Tick.OnTick -= UpdateItem;
            Debug.Log("[Painter] I destroyed your asshole");
            if (item == null) return;
            item = null;
        }
    
    }

}
