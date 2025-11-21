using System;
using System.Linq;
using System.Collections.Generic;
using NaughtyAttributes;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
namespace Journal
{

    [CreateAssetMenu(fileName = "RecipesEntry", menuName = "RecipesEntry")]
    public class RecipesEntry : ScriptableObject
    {
        public List<RecipesText> texts;
        public void SetUnlocked()
        {
            foreach (RecipesText text in texts)
            {
                text.IsUnlocked = text.levelToUnlock <= PlayerProgression.Level;
            }
        }
    }
    [Serializable]
    public class RecipesText
    {   
        public int levelToUnlock;
        [ResizableTextArea]
        public string text;
        private bool isUnlocked;
        public bool IsUnlocked { get => isUnlocked; set => isUnlocked = value; }
    }
}