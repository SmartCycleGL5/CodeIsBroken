
using System;
using AYellowpaper.SerializedCollections;
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
        [ResizableTextArea]
        public string explanation;

        [ResizableTextArea]
        public string codeShowcase;
        public bool bHintTaken;
        [SerializedDictionary("Style", "Text")]
        public SerializedDictionary<JournalStyle,JournalText> jText;

    }
    public enum JournalStyle
    {
        explanation,
        code
    }
    [Serializable]
    public struct JournalText
    {
        [ResizableTextArea]
        public string text;
    }
}