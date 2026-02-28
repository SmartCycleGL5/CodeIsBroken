using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using CodeIsBroken.Coding;
using CodeIsBroken.UI.Window;
using UnityEngine;
using UnityEngine.UIElements;

namespace CodeIsBroken.UI
{
    public class FileBrowser : MonoBehaviour, IInspectorElement
    {
        private VisualElement root;
        Inspector Inspector;
        
        public static FileBrowser New(Inspector inspector)
        {
            FileBrowser fileBrowser = inspector.gameObject.AddComponent<FileBrowser>();
            
            fileBrowser.Inspector = inspector;
            fileBrowser.root = InspectorManager.FileBrowserUI.Instantiate();
            
            inspector.root.Q("Holder").Add(fileBrowser.root);
            
            fileBrowser.DisplayFiles();

            fileBrowser.root.Q<Button>("Create").clicked += fileBrowser.CreateScript;
            
            return fileBrowser;
        }

        private async void CreateScript()
        {
            string name = await WindowManager.OpenEnterValue("Name the script");

            while(!isValidName(name))
            {
                name = await WindowManager.OpenEnterValue("<color=#ff0000>Enter a valid name</color>");
            }
            
            Inspector.programmable.AddScript(
                script: new Script(
                    data: PersistentData<string>.NewFile(name, Script.DefaultScriptFolder, "cs",Script.DefaultCode(name, Inspector.programmable.toDeriveFrom)), 
                    machine: Inspector.programmable), 
                autoOpen: true);
            
            Inspector.Refresh();
            Inspector.Focus();
            Close();
                
            bool isValidName(string name)
            {
                foreach (var item in PersistentData<string>.disallowedNames)
                {
                    if (name == item) return false;
                }
                foreach (var item in PersistentData<string>.disallowedCharacters)
                {
                    if (name.ToCharArray().Contains(item))
                        return false;
                }
                /* replace with file check
                if(Compiler.usedScripts.ContainsKey(name))
                {
                    return false;
                }*/

                return true;
            }
        }

        public void DisplayFiles()
        {
            if (!Directory.Exists(Application.persistentDataPath + "/" + Script.DefaultScriptFolder)) return;
            DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/" + Script.DefaultScriptFolder);

            foreach (var file in info.GetFiles())
            {
                VisualElement fileUI = InspectorManager.FileUI.Instantiate();
                fileUI.Q<Button>().text = file.Name;//.Substring(0, file.Name.LastIndexOf('.'));
                fileUI.Q<Button>().clicked += () =>
                {
                    try
                    {
                        Inspector.programmable.AddScript(new Script(PersistentData<string>.LoadFile(file.FullName), Inspector.programmable));
                    }
                    catch (Exception e)
                    {
                        Debug.LogWarning(e.Message);
                        fileUI.Q<Button>().text = "<color=red>failed</color>";
                    }
                    
                    Inspector.Refresh();
                    Inspector.Focus();
                    Close();
                };
                
                root.Q("FileHolder").Add(fileUI);
            }
        }

        private void OnDestroy()
        {
            Inspector.root.Q("Holder").Remove(root);
            root.Q<Button>("Create").clicked -= CreateScript;
        }

        public void Close()
        {
            Destroy(this);
        }
    }
}
