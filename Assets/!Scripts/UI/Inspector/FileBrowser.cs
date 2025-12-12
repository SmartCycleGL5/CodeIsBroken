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
    public class FileBrowser : MonoBehaviour
    {
        private VisualElement root;
        Inspector Inspector;
        
        public static FileBrowser NewFilebrowser(Inspector inspector)
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
            
            Inspector.programmable.AddScript(new Script(PersistentData<string>.NewFile(name, Script.DefaultScriptFolder, "cs",Script.DefaultCode(name, Inspector.programmable.toDeriveFrom)), Inspector.programmable));
            
            Inspector.Refresh();
                
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
                if(Compiler.activePlayerScripts.ContainsKey(name))
                {
                    return false;
                }

                return true;
            }
        }

        public void DisplayFiles()
        {
            DirectoryInfo info = new DirectoryInfo(Application.persistentDataPath + "/Scripts");

            foreach (var file in info.GetFiles())
            {
                VisualElement fileUI = InspectorManager.FileUI.Instantiate();
                fileUI.Q<Button>().text = file.Name;//.Substring(0, file.Name.LastIndexOf('.'));
                fileUI.Q<Button>().clicked += () =>
                {
                    Inspector.programmable.AddScript(new Script(PersistentData<string>.LoadFile(file.FullName), Inspector.programmable));
                    Inspector.Refresh();
                    Destroy(this);
                };
                
                root.Q("FileHolder").Add(fileUI);
            }
        }

        private void OnDestroy()
        {
            Inspector.root.Q("Holder").Remove(root);
            root.Q<Button>("Create").clicked -= CreateScript;
        }
    }
}
