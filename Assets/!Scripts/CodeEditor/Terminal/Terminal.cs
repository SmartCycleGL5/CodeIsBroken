using System;
using CodeIsBroken.UI.Window;
using SharpCube.Highlighting;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeIsBroken.Coding;
using ScriptEditor.Console;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI.Window.CodeEditor
{
    [DefaultExecutionOrder(1000)]
    public class Terminal : CodeEditor.Element
    {
        public bool isFocused
        {
            get
            {
                if (input == null) return false;
                try
                {
                    return input ==  input.panel.focusController.focusedElement;
                }
                catch
                {
                    return false;
                }
            }
        }
        
        CodeEditor editor;
        
        public SyntaxHighlighting activeHighlighting = new();
        public Action<string> save; 
        
        //Label inheritedMembers;
        //Label inheritedClass;
        
        TextField input;


        public override void Initialize(CodeEditor editor)
        {
            this.editor = editor;
            
            activeHighlighting.SetPallate(ColorThemes.ActivePallate);
            
            input = editor.editorRoot.Q<TextField>("Input");
            input.RegisterCallback<FocusOutEvent>(OnLoseFocus);
            input.Q<TextElement>().enableRichText = true;

            Load();
        }
        
        async void OnLoseFocus(FocusOutEvent evt)
        {
            await Save();
        }

        public override async void Close()
        {
            await Save();
            input.UnregisterCallback<FocusOutEvent>(OnLoseFocus);
        }

        public void Load()
        {
            if (editor.script == null) return;

            Debug.Log(editor.script);

            //inheritedMembers.text = "";
            
            HighlightCode();

            PlayerConsole.Clear();
            foreach (var error in Compiler.compilerErrors)
            {
                PlayerConsole.LogError(error.error.ToString(), error.source.name);
            }
        }

        public async Task Save()
        {
            if (editor.script == null) return;
            
            RemoveHighlight();

            if (editor.script.data != input.text)
            {
                PlayerConsole.Log("Saving...", editor.script.name);

                editor.script.UpdateData(input.text);

                PlayerConsole.Log("Saved!", editor.script.name);
            }

            //window.Rename(scriptToEdit.name);

            HighlightCode();
        }
        
        void HighlightCode()
        {
            input.value = activeHighlighting.HighlightCode(input.text);
        }
        void RemoveHighlight()
        {
            input.value = activeHighlighting.RemoveHighlight(input.text);
        }
    }
}
