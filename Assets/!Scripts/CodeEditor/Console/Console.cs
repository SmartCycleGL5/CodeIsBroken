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
            
            Label MembersText = extentionRoot.Q<Label>("Members");
            MembersText.text = "Variables: \n";
            foreach (var variable in editor.script.connectedMachine.variableInfo)
            {
                string returnType = SimplifyTypeText(variable.FieldType.ToString());
                
                MembersText.text += returnType + " " + variable.Name + " \n";
            }

            MembersText.text += "\nMethods: \n";
            foreach (var method in editor.script.connectedMachine.methodInfo)
            {
                string returnType = SimplifyTypeText(method.ReturnType.ToString());

                string parameters = "";

                var paramInfo = method.GetParameters();
                for (int i = 0; i < paramInfo.Length; i++)
                {
                    var parameter = paramInfo[i];
                    if(i > 0) parameters += ", "; 
                    parameters += SimplifyTypeText(parameter.ParameterType + " " + parameter.Name);
                }
                
                MembersText.text += $"{returnType} {method.Name}({parameters}) \n";
            }

            string SimplifyTypeText(string text)
            {
                text = text.Replace("System.", "");
                text = text.Replace("CodeIsBroken.", "");
                text = text.Replace("Int32", "int");
                text = text.Replace("Boolean", "bool");
                text = text.Replace("Single", "float");
                text = text.Replace("String", "string");
                text = text.Replace("Void", "void");
                
                return text;
            }
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
