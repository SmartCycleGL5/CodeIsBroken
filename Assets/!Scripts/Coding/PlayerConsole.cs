using System;
using System.Runtime.Remoting.Contexts;
using UnityEngine;

namespace ScriptEditor.Console
{
    public static class PlayerConsole
    {
        public static Action<object> LogEvent;

        public static void Clear()
        {
            LogEvent?.Invoke("/Clear");
        }

        public static void Log(object message, string context)
        {
            object msg = $"[{context}] " + message;
            Debug.Log("[PlayerConsole] " + msg);
            LogEvent?.Invoke(msg);
        }
        public static void LogWarning(object message, string context)
        {
            object msg = $"[{context}] <color=yellow>{message}</color>";
            Debug.Log("[PlayerConsole] " + msg);
            LogEvent?.Invoke(msg);
        }
        public static void LogError(object message, string context, bool throwException = false)
        {
            object msg = $"[{context}] <color=red>{message}</color>";
            LogEvent?.Invoke(msg);

            if (throwException)
                throw new Exception($"[PlayerConsole] user error: {msg}");
        }
    }
}
