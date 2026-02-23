using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI
{
    public class ScriptEditor : MonoBehaviour, IInspectorElement
    {
        private VisualElement root;
        Inspector Inspector;
        private Script toEdit;
        
        public static ScriptEditor New(Inspector inspector, Script toEdit)
        {
            ScriptEditor scriptEditor = inspector.gameObject.AddComponent<ScriptEditor>();
            
            scriptEditor.Inspector = inspector;
            scriptEditor.root = InspectorManager.ScriptEditorUI.Instantiate();
            inspector.root.Q("Holder").Add(scriptEditor.root);

            scriptEditor.toEdit = toEdit;
            scriptEditor.root.Q<Button>("Remove").clicked += scriptEditor.RemoveScript;
            
            return scriptEditor;
        }

        private void RemoveScript()
        {
            toEdit.Delete();
            Close();
        }

        private void OnDestroy()
        {
            root.Q<Button>("Remove").clicked -= RemoveScript;
            Inspector.root.Q("Holder").Remove(root);
        }

        public void Close()
        {
            Destroy(this);
        }
    }
}
