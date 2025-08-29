using UnityEngine;

public class HelloFromSourceGenerator : MonoBehaviour
{
    static string thing()
    {
        return Generators.Test.test.GetText();
    }  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(thing());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
