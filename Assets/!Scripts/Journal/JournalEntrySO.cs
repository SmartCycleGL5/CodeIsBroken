
using UnityEngine;
namespace Journal
{

    [CreateAssetMenu(fileName = "JournalEntrySO", menuName = "Scriptable Objects/JournalEntrySO")]
    public class JournalEntrySO : ScriptableObject
    {
        public string title;
        public Texture2D showcaseI;
        public string explanation;

        public string codeShowcase;
}
}