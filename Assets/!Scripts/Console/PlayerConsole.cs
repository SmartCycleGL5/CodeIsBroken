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
        public static void LogError(object message)
        {
            object msg = $"[{Tick.tick}] <color=red>{message}</color>";
            Debug.Log("[PlayerConsole] " + msg);
            LogEvent?.Invoke(msg);
        }
    }
}
