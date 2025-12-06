using ScriptEditor.Console;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI.Window.CodeEditor
{
    public class Console : CodeEditor.Element
    {
        Label output;
        
        public override void Initialize(CodeEditor editor)
        {
            output = editor.editorRoot.Q<Label>("Output");
            PlayerConsole.LogEvent += Log;
        }

        public override void Close()
        {
            
        }

        private void Log(object obj)
        {
            if (obj is string)
            {
                switch ((string)obj)
                {
                    case "/Clear":
                    {
                        output.text = "";
                        return;
                    }
                }
            }

            output.text += obj + "\n";
        }
    }
}
