using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static WindowManager;

namespace WindowSystem
{
    public class Terminal : MonoBehaviour, IWindow
    {
        public Script scriptToEdit { get; private set; }

        static VisualTreeAsset terminalAsset;
        public static bool findingAsset;

        public static List<Terminal> terminals = new();
        public static bool focused
        {
            get
            {
                if (terminals.Count <= 0) return false;
                foreach (var terminal in terminals)
                {
                    if (terminal.isFocused)
                        return true;
                }
                return false;
            }
        }

        //UI elements
        Dictionary<string, Button> buttons = new();
        VisualElement terminal;
        Label availableMethods;
        TextField input;
        Focusable focusedElement => input.panel.focusController.focusedElement;

        public bool isFocused
        {
            get
            {
                if (input == null) return false;
                try
                {
                    return input == focusedElement;
                }
                catch
                {
                    return false;
                }
            }
        }
        public Window window { get; set; }


        private async void Start()
        {
            if (terminalAsset == null)
            {
                Debug.Log("[Terminal] getting asset");

                findingAsset = true;
                terminalAsset = await Addressable.LoadAsset<VisualTreeAsset>(AddressableAsset.Terminal, AddressableToLoad.GameObject);
                findingAsset = false;
            }


            //Debug.LogError("[Terminal] " + terminalAsset);

            terminal = terminalAsset.Instantiate();
            window = new Window(scriptToEdit.name, terminal, this);

            availableMethods = terminal.Q<Label>("Methods");
            input = terminal.Q<TextField>("Input");

            input.RegisterCallback<FocusOutEvent>(OnLoseFocus);

            terminals.Add(this);

            Load();
        }

        private void OnDestroy()
        {
            terminals.Remove(this);
            input.UnregisterCallback<FocusOutEvent>(OnLoseFocus);
        }
        void OnLoseFocus(FocusOutEvent evt)
        {
            Debug.Log("deselect");
            Save();
        }

        public void Close()
        {
            Save();
            Destroy(this);
        }

        public void Load()
        {
            if (scriptToEdit == null) return;

            Debug.Log(scriptToEdit);

            input.value = scriptToEdit.rawCode;

            availableMethods.text = "Available Methods: \n";

            if(scriptToEdit.connectedMachine != null)
                DisplayIntegratedMethods();
        }
        void DisplayIntegratedMethods()
        {
            foreach (var method in scriptToEdit.connectedMachine.IntegratedMethods)
            {
                availableMethods.text += "\n" + method.Value.toCall.ReturnType.Name + " " + method.Value.toCall.Name + "(";

                foreach (var item in method.Value.toCall.GetParameters())
                {
                    availableMethods.text += item.ParameterType.Name + " " + item.Name;
                }

                availableMethods.text += ");";
            }
        }

        public bool Save()
        {
            if (scriptToEdit == null || ScriptManager.isRunning) return false;

            try
            {
                scriptToEdit.Compile(input.text);
                window.Rename(scriptToEdit.name);
                return true;
            }
            catch
            {
                Debug.LogWarning("[Terminal] Couldnt Save");
                return false;
            }
        }

        public static Terminal NewTerminal(Script script)
        {
            Terminal newTerminal = UIManager.Instance.gameObject.AddComponent<Terminal>();

            newTerminal.scriptToEdit = script;

            return newTerminal;
        }
    }
}
