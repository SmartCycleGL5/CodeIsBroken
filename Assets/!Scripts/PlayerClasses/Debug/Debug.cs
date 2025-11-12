using ScriptEditor.Console;
using UnityEngine;

namespace CodeIsBroken
{
    public static class Debug
    {
        public static void Log(object message)
        {
            PlayerConsole.Log(message);
        }
        public static void LogWarning(object message)
        {
            PlayerConsole.LogWarning(message);
        }
        public static void LogError(object message)
        {
            PlayerConsole.LogError(message);
        }
    }
}
