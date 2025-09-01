using TMPro;
using UnityEngine;

public class SyntaxHighlighting : MonoBehaviour
{
    [SerializeField] TMP_InputField syntax;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        syntax.text = "<color=\"blue\">im blue</color>";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
