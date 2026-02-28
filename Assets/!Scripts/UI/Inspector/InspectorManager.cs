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
        public static VisualTreeAsset FileUI { get; private set; }
        public static VisualTreeAsset FileBrowserUI { get; private set; }
        public static VisualTreeAsset ScriptEditorUI { get; private set; }
        
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
            if (FileBrowserUI == null)
            {
                FileBrowserUI = await Utility.Addressable.LoadAsset<VisualTreeAsset>("FileBrowserUI");
            }
            if (ScriptEditorUI == null)
            {
                ScriptEditorUI = await Utility.Addressable.LoadAsset<VisualTreeAsset>("ScriptEditorUI");
            }
            if (FileUI == null)
            {
                FileUI = await Utility.Addressable.LoadAsset<VisualTreeAsset>("FileUI");
            }
        }

        public static Inspector NewInspector(Programmable programmable)
        {
            Inspector inspector = Instantiate(InspectorUI, programmable.transform);
            inspector.transform.position += Vector3.up * programmable.inspectorHeight;
            inspector.programmable = programmable;
            return inspector;
        }
    }
}
