using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
namespace Journal
{

    [CreateAssetMenu(fileName = "JournalEntrySO", menuName = "Scriptable Objects/JournalEntrySO")]
    public class JournalEntrySO : ScriptableObject
    {
        [ResizableTextArea]
        public string title;
        [ShowAssetPreview]
        public Texture2D showcaseI;
        public bool bHintTaken;
        public List<JournalText> journalTexts;

    }
    public enum JournalStyle
    {
        explanation,
        code
    }
    [Serializable]
    public struct JournalText
    {
        public JournalStyle style;
        [ResizableTextArea]
        public string text;
    }
}