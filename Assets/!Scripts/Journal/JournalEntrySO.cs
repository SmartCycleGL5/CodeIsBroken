
using UnityEngine;
namespace Journal
{

    [CreateAssetMenu(fileName = "JournalEntrySO", menuName = "Scriptable Objects/JournalEntrySO")]
    public class JournalEntrySO : ScriptableObject
    {
        [NaughtyAttributes.ResizableTextArea]
        public string title;
        [NaughtyAttributes.ShowAssetPreview]
        public Texture2D showcaseI;
        [NaughtyAttributes.ResizableTextArea]
        public string explanation;
        [NaughtyAttributes.ResizableTextArea]
        public string codeShowcase;
        public bool bHintTaken;
}
}