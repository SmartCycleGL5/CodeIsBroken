using ScriptEditor.Console;
using UnityEngine;

namespace CodeIsBroken
{
    public static class Console
    {
        public static void WriteLine(object message)
        {
            PlayerConsole.Log(message);
        }
        public static void WriteWarning(object message)
        {
            PlayerConsole.LogWarning(message);
        }
        public static void WriteError(object message)
        {
            PlayerConsole.LogError(message);
        }
    }
}
