using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI
{
    public class InspectorManager : MonoBehaviour
    {
        public static InspectorManager Instance { get; private set; }
        public static Inspector InspectorUI { get; private set; }
        public static VisualTreeAsset ScriptUI { get; private set; }

        [SerializeField] private float height;
        
        private void Awake()
        {
            Instance = this;
        }

        private async void Start()
        {
            if (InspectorUI == null)
            {
                InspectorUI = await Utility.Addressable.LoadAsset<Inspector>("Inspector", true);
            }
            if (ScriptUI == null)
            {
                ScriptUI = await Utility.Addressable.LoadAsset<VisualTreeAsset>("ScriptUI");
            }
        }

        public static Inspector NewInspector(Programmable programmable)
        {
            Inspector inspector = Instantiate(InspectorUI, programmable.transform);
            inspector.transform.position += Vector3.up * Instance.height;
            inspector.programmable = programmable;
            return inspector;
        }
    }
}
