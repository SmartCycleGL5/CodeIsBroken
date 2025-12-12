using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI
{
    public class Inspector : MonoBehaviour
    {
        public static Inspector activeInspector { get; private set; }
        public VisualElement root { get; set; }
        public Programmable programmable;

        public bool focused => root.Contains((VisualElement)root.focusController.focusedElement);

        VisualElement scriptHolder;
        
        private void Start()
        {

            if (activeInspector != null)
            {
                Destroy(activeInspector.gameObject);    
            }
            
            activeInspector = this;
            root = GetComponent<UIDocument>().rootVisualElement;
            scriptHolder = root.Q("ScriptHolder");

            Refresh();

            root.Q<Button>("AddScript").clicked += AddScript;
            
            root.Q("Holder").Focus();
        }

        private void Update()
        {
            if(!focused)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            root.Q<Button>("AddScript").clicked -= AddScript;
        }

        public void Refresh()
        {
            int childCount = scriptHolder.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                scriptHolder.RemoveAt(i);
            }
            
            foreach (var script in programmable.attachedScripts)
            {
                VisualElement scriptUI = InspectorManager.ScriptUI.Instantiate();
                scriptUI.Q<Button>("Script").text = script.name;
                scriptHolder.Add(scriptUI);
                
                scriptUI.Q<Button>("Edit").clicked += script.Edit;
            }
        }

        private void AddScript()
        {
            FileBrowser.NewFilebrowser(this);
        }
        
        
    }
}
