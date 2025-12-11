using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI
{
    public class Inspector : MonoBehaviour
    {
        public static Inspector activeInspector { get; private set; }
        private VisualElement root;
        public Programmable programmable;

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

            UpdateScripts();

            root.Q<Button>("AddScript").clicked += AddScript;
        }

        private void OnDestroy()
        {
            root.Q<Button>("AddScript").clicked -= AddScript;
        }

        private void UpdateScripts()
        {
            int childCount = scriptHolder.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                scriptHolder.RemoveAt(i);
            }
            
            foreach (var script in programmable.attachedScripts)
            {
                VisualElement scriptUI = InspectorManager.ScriptUI.Instantiate();
                scriptUI.Q<Label>("Name").text = script.name;
                scriptHolder.Add(scriptUI);
                
                scriptUI.Q<Button>("Edit").clicked += script.Edit;
            }
        }

        private async void AddScript()
        {
            await programmable.AddScript();
            
            UpdateScripts();
        }
        
        
    }
}
