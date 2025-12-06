using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI.Window.CodeEditor
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

        public List<Element> editorElements = new List<Element>()
        {
            new Terminal(),
            new Console(),
        };
        public VisualElement editorRoot;
        
        public Script script;
        
        public CodeEditor(Script script, bool requestClose = true, IWindow window = null) : base(script.name, requestClose, window)
        {
            TerminalManager.editors.Add(this);

            editorRoot = TerminalManager.terminalUI.Instantiate();
            tab.Add(editorRoot);

            foreach (var item in editorElements)
            {
                item.Initialize(this);   
            }
        }

        public abstract class Element
        {
            public abstract void Initialize(CodeEditor editor);
            public abstract void Close();
        }
    }
}
