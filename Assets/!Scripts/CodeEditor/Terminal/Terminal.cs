using System;
using CodeIsBroken.UI.Window;
using SharpCube.Highlighting;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeIsBroken.Coding;
using ScriptEditor.Console;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.IDE
{
    [Serializable]
    public class Terminal : IDEExtention
    {
        CodeEditor editor;
        
        public SyntaxHighlighting activeHighlighting = new();
        public Action<string> save; 
        
        TextField input;


        public override string uiPath => "Window/CodeEditor/Terminal";

        public override void Initialize(CodeEditor editor)
        {
            base.Initialize(editor);
            
            this.editor = editor;
            
            activeHighlighting.SetPallate(ColorThemes.ActivePallate);
            
            input = extentionRoot.Q<TextField>("Input");
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

        public override IDEExtention Clone()
        {
            return (Terminal)MemberwiseClone();
        }

        public void Load()
        {
            if (editor.script == null) return;
            
            input.value = editor.script.data;
            
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
                await Compiler.StartCompileAsync();

                PlayerConsole.Log("Saved!", editor.script.name);
            }

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
