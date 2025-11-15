
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
            Metrics.instance.UseElectricity(1);
            Debug.Log("Set color to: " + color);
            if (item == null) return;
            painterConveyor.PaintEffect();
            item.definition.Modify(new CodeIsBroken.Product.Modification.Color(new UnityEngine.Color(color.r, color.g, color.b, color.a)));
        }
    
        void OnDestroy()
        {
            if (item == null) return;
            Destroy(item);
        }
    
    }

}
