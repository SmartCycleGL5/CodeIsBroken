using System;
using UnityEngine;

namespace SharpCube
{
    public static class PlayerConsole
    {
        public static Action<object> LogEvent; 

        public static void Log(object message)
        {
            object msg = $"[{Tick.tick}] " + message;
            Debug.Log("[PlayerConsole] " + msg);
            LogEvent?.Invoke(msg);
        }
        public static void LogWarning(object message)
        {
            object msg = $"[{Tick.tick}] <color=yellow>{message}</color>";
            Debug.Log("[PlayerConsole] " + msg);
            LogEvent?.Invoke(msg);
        }
        public static void LogError(object message, bool throwException = true)
        {
            object msg = $"[{Tick.tick}] <color=red>{message}</color>";
            LogEvent?.Invoke(msg);

            if (throwException)
                throw new Exception($"[PlayerConsole] user error: {msg}");
        }
    }
}
