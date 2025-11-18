using ScriptEditor.Console;
using UnityEngine;

namespace CodeIsBroken
{
    public static class Console
    {
        public static void WriteLine(object message)
        {
            PlayerConsole.Log(message, Tick.tickCount.ToString());
        }
        public static void WriteWarning(object message)
        {
            PlayerConsole.LogWarning(message, Tick.tickCount.ToString());
        }
        public static void WriteError(object message)
        {
            PlayerConsole.LogError(message, Tick.tickCount.ToString());
        }
    }
}
