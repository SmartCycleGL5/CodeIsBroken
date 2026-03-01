using System;
using System.IO;
using UnityEngine;

namespace CodeIsBroken
{
    public class ScriptsDeletor : MonoBehaviour
    {
        void Start()
        {
            InputReader.deleteScripts += DeleteScripts;
        }
        

        private void OnDestroy()
        {
            InputReader.deleteScripts -= DeleteScripts;
        }
        
        private void DeleteScripts()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/" + Script.DefaultScriptFolder)) return;
            DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/" + Script.DefaultScriptFolder);

            foreach (var file in info.GetFiles())
            {
                file.Delete();
            }
            
            PersistentData<string>.refresh.Invoke();
        }
    }
}
