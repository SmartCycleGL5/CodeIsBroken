using AYellowpaper.SerializedCollections;
using System;
using UnityEngine;

public class ColorThemes : MonoBehaviour
{
    public static ColorPallate ActivePallate;
    public static ColorThemes Instance { get; private set; }

    public static Action<ColorPallate> UpdatedPallate;

    [SerializedDictionary("Theme", "Pallate")]
    public SerializedDictionary<string, ColorPallate> Themes = new()
    {
        { "Default", new ColorPallate() }
    };

    private void Start()
    {
        Instance = this;
        ActivePallate = Themes["Default"];
    }

    public static void SwapPallate(ColorPallate pallate)
    {
        ActivePallate = pallate;
        UpdatedPallate?.Invoke(ActivePallate);
    }
}
