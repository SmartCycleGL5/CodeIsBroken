using System;
using System.Collections.Generic;
using CodeIsBroken.UI.Window;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.IDE
{
    public class CodeEditor : WindowElement
    {
        public bool isFocused
        {
            get
            {
                return false;
            }
        }

        public static VisualTreeAsset codeEditorUI;
        
        public static List<IDEExtention> extentions => IDEManager.instance.CodeEditorExtentions;
        public VisualElement editorRoot;
        public PersistentData<string> script { get; private set; }
        
        public CodeEditor(PersistentData<string> script, bool requestClose = true) : base(script.name, requestClose)
        {
            this.script = script;
            
            Debug.Log(script);
            
            IDEManager.editors.Add(this);

            editorRoot = codeEditorUI.Instantiate();
            editorRoot.style.height = 2000;
            tab.Add(editorRoot);

            foreach (var item in extentions)
            {
                item.Initialize(this);   
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
    }
}
