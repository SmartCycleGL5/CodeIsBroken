using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
namespace Journal
{

    [CreateAssetMenu(fileName = "JournalEntrySO", menuName = "Scriptable Objects/JournalEntrySO")]
    public class JournalEntrySO : ScriptableObject, IComparable
    {
        [ResizableTextArea]
        public string title;
        [ShowAssetPreview]
        public Texture2D showcaseI;
        public bool bHintTaken;
        public List<JournalText> journalTexts;
        public int sortOrder;

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            JournalEntrySO otherEntry = obj as JournalEntrySO;
            if (otherEntry != null)
                return this.sortOrder.CompareTo(otherEntry.sortOrder);
            else
                throw new ArgumentException("Object is not a JournalEntrySO");
        }
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