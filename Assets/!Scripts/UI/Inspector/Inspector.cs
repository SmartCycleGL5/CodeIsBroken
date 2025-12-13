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
        
        public IInspectorElement inspectorElement;
        
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

            root.Q<Button>("AddScript").clicked += OpenFileBrowser;
            
            Focus();
        }

        private void FixedUpdate()
        {
            if(!focused)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            root.Q<Button>("AddScript").clicked -= OpenFileBrowser;
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

        public void Focus()
        {
            root.Q("Holder").Focus();
        }

        private void OpenFileBrowser()
        {
            if(inspectorElement != null) inspectorElement.Close();
            
            inspectorElement = FileBrowser.NewFilebrowser(this);
        }
    }
}
