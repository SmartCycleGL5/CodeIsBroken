using System;
using CodeIsBroken.Coding;
using CodeIsBroken.IDE;
using ScriptEditor.Console;
using UnityEngine;
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

            foreach (var error in Compiler.compilerErrors)
            {
                Log($"[{error.source.name}] <color=red>{error.error.ToString()}</color>");
            }
            
            Debug.Log("Console initialized");
        }

        public override void Close()
        {
            PlayerConsole.LogEvent -= Log;
        }
        
        public override IDEExtention Clone()
        {
            return (Console)MemberwiseClone();
        }

        private void Log(object obj)
        {
            if (obj is string s)
            {
                switch (s)
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
