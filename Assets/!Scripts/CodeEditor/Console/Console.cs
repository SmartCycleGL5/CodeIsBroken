using System;
using CodeIsBroken.IDE;
using ScriptEditor.Console;
using UnityEngine.UIElements;

namespace CodeIsBroken.IDE
{
    [Serializable]
    public class Console : IDEExtention
    {
        Label output;
        
        public override string uiPath => "Window/CodeEditor/Console";
        
        public override void Initialize(CodeEditor editor)
        {
            base.Initialize(editor);
            
            output = extentionRoot.Q<Label>("Output");
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
