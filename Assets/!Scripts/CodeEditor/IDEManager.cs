using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Utility;

namespace CodeIsBroken.IDE
{
    public class IDEManager : MonoBehaviour
    {
        public static IDEManager instance { get; private set; }
        
        [field: SerializeField, SerializeReference, SubclassSelector]
        public List<IDEExtention> CodeEditorExtentions { get; private set; } = new();
        
        public static List<CodeEditor> editors = new();
        

        private void Awake()
        {
            instance = this;
        }

        async void Start()
        {
            if (CodeEditor.codeEditorUI == null)
            {
                CodeEditor.codeEditorUI = await Addressable.LoadAsset<VisualTreeAsset>("Window/CodeEditor");
            }

            foreach (var extention in CodeEditorExtentions)
            {
                if (extention.extentionUI == null)
                {
                    extention.extentionUI = await Addressable.LoadAsset<VisualTreeAsset>(extention.uiPath);
                }
            }
        }
    }

}
