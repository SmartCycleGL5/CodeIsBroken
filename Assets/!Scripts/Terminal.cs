using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public TMP_InputField input;
    public Interpreter interpreter = new();

    public string[] lines;

    public void Run()
    {
        lines = ExtractLines(input.text);
        interpreter.Interperate(ExtractLines(input.text));
    }

    string[] ExtractLines(string raw)
    {
        string modified = raw.Replace("\n", "");
        return modified.Split(';', System.StringSplitOptions.RemoveEmptyEntries);
    }
}
