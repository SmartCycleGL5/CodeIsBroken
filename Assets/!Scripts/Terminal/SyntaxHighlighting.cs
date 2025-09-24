using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class SyntaxHighlighting : MonoBehaviour
{

    [SerializedDictionary("Syntax", "Color")]
    public SerializedDictionary<string, Color> syntax;

    [SerializeField] TMP_InputField input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input.text = "<color=\"blue\">im blue</color>";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
