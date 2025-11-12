using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ColorThemes : MonoBehaviour
{
    public static ColorThemes Instance { get; private set; }

    [SerializedDictionary("Theme", "Pallate")]
    public SerializedDictionary<string, ColorPallate> Themes = new()
    {
        { "Default", new ColorPallate() }
    };

    private void Start()
    {
     Instance = this;   
    }
}
