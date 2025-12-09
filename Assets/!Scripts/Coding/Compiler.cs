using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoslynCSharp;
using ScriptEditor.Console;
using UnityEngine;

namespace CodeIsBroken.Coding
{
    public class Compiler : MonoBehaviour
    {
        public static ScriptDomain scriptDomain;
        public static Dictionary<string, Script> activePlayerScripts = new();
        
        public static bool compiling;
        public static List<Error> compilerErrors = new List<Error>();
        

        private void Start()
        {
            scriptDomain = new ScriptDomain();
        }

        public static void StartCompile()
        {
            _=StartCompileAsync();
        }

        public static async Task<bool> StartCompileAsync()
        {
            if(compiling) return false;
            compiling = true;
            
            if(GameManager.isRunning)
                GameManager.StopMachines();
            
            GameManager.runButton.text = "Compiling...";
            GameManager.runButton.SetEnabled(false);
            
            bool result = await Compile();

            if (result)
            {
                GameManager.runButton.text = "Start";
                GameManager.runButton.SetEnabled(true);
            }
            else
            {
                GameManager.runButton.text = "<color=red>Failed</color>";
            }
            
            compiling = false;
            return result;
        }

        static async Task<bool> Compile()
        {
            bool success = true;

            compilerErrors = new();

            foreach (var script in activePlayerScripts)
            {
                if (!script.Value.Compile(ref compilerErrors))
                    success = false;

                await Task.Delay(10);
            }

            PlayerConsole.Clear();

            if (!success)
            {
                foreach (var error in compilerErrors)
                {
                    PlayerConsole.LogError(error.error.ToString(), error.source.name);
                }
            }

            return success;
        }
    }
    
    public struct Error
    {
        public Script source;
        public CompileError error;

        public Error (Script source, CompileError error)
        {
            this.source = source;
            this.error = error;
        }
    }
}
