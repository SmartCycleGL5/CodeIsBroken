using System;
using System.Collections.Generic;
using CodeIsBroken.UI.Window;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.IDE
{
    public class CodeEditor : WindowElement
    {
        public static bool focused
        {
            get
            {
                if (IDEManager.editors.Count <= 0) return false;
                foreach (var terminal in IDEManager.editors)
                {
                    if (terminal.isFocused)
                        return true;
                }
                return false;
            }
        }
        public bool isFocused => editorRoot.Contains((VisualElement)editorRoot.focusController.focusedElement);

        public static VisualTreeAsset codeEditorUI;
        
        public List<IDEExtention> extentions = new();
        public VisualElement editorRoot;
        public PersistentData<string> script { get; private set; }
        
        public CodeEditor(PersistentData<string> script, bool requestClose = true) : base(script.name, requestClose)
        {
            this.script = script;
            
            IDEManager.editors.Add(this);

            editorRoot = codeEditorUI.Instantiate();
            editorRoot.style.height = 2000;
            tab.Add(editorRoot);

            foreach (var extention in IDEManager.instance.CodeEditorExtentions)
            {
                extentions.Add(extention.Clone());
            }

            foreach (var extention in extentions)
            {
                extention.Initialize(this);   
            }
        }

        protected override void Closing()
        {
            base.Closing();

            foreach (var extention in extentions)
            {
                extention.Close();
            }
        }
    }
    
    [Serializable]
    public abstract class IDEExtention
    {
        [SerializeField] private int UI_weight = 1;
        
        [HideInInspector] public VisualTreeAsset extentionUI;
        protected TemplateContainer extentionRoot;
        
        public abstract string uiPath { get; }

        public virtual void Initialize(CodeEditor editor)
        {
            extentionRoot = extentionUI.Instantiate();
            editor.editorRoot.Add(extentionRoot);
            extentionRoot.style.height = UI_weight * 1000;
        }

        public abstract void Close();

        public abstract IDEExtention Clone();
    }
}
