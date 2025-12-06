using CodeIsBroken.UI.Window;
using SharpCube.Highlighting;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeIsBroken.Coding;
using Mono.Cecil.Cil;
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
        public Script scriptToEdit { get; private set; }
        
        Label inheritedMembers;
        Label inheritedClass;
        
        TextField input;


        public override void Initialize(CodeEditor editor)
        {
            this.editor = editor;
            
            activeHighlighting.SetPallate(ColorThemes.ActivePallate);
            
            input = editor.editorRoot.Q<TextField>("Input");
            input.RegisterCallback<FocusOutEvent>(OnLoseFocus);
            input.Q<TextElement>().enableRichText = true;
            
            scriptToEdit.Deleted += editor.ForceClose;

            Load();
        }
        
        async void OnLoseFocus(FocusOutEvent evt)
        {
            await Save();
        }

        public override async void Close()
        {
            await Save();
            scriptToEdit.Deleted -= editor.ForceClose;
            input.UnregisterCallback<FocusOutEvent>(OnLoseFocus);
        }

        public void Load()
        {
            if (scriptToEdit == null) return;

            Debug.Log(scriptToEdit);

            input.value = scriptToEdit.rawCode;

            inheritedMembers.text = "";
            
            HighlightCode();

            PlayerConsole.Clear();
            foreach (var error in Compiler.compilerErrors)
            {
                PlayerConsole.LogError(error.error.ToString(), error.source.name);
            }
        }

        public async Task Save()
        {
            if (scriptToEdit == null) return;

            if (GameManager.isRunning)
            {
                GameManager.StopMachines();
            }

            RemoveHighlight();

            if (scriptToEdit.rawCode != input.text)
            {
                PlayerConsole.Log("Saving...", scriptToEdit.name);

                await scriptToEdit.Save(input.text);

                PlayerConsole.Log("Saved!", scriptToEdit.name);
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
