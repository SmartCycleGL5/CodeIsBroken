using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI
{
    [DefaultExecutionOrder(100)]
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
    
        public static VisualElement canvas { get; private set; }
        
        private void Awake()
        {
            Instance = this;
    
            canvas = GetComponent<UIDocument>().rootVisualElement;
        }
    }
}

