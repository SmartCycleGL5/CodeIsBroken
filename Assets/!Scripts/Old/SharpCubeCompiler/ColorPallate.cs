using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ColorPallate
{
    public enum Type
    {
        defaultColor,
        backgroundColor,
        initializerColor,
        modifierColor,
        conditionalColor,
        referenceColor,
        loopColor,
        jumpColor,
    }

    public SerializedDictionary<Type, Color> Colors = new SerializedDictionary<Type, Color>()
    {
        { Type.defaultColor, new Color(1, 1, 1) },
        { Type.backgroundColor, new Color(0, 0, 0) },
        { Type.initializerColor, new Color(.325f, .592f, .816f) },
        { Type.modifierColor, new Color(.325f, .592f, .816f) },
        { Type.conditionalColor, new Color(.325f, .592f, .816f) },
        { Type.referenceColor, new Color(.325f, .592f, .816f) },
        { Type.jumpColor, new Color(0.9f, 0.5f, 0.961f) },
        { Type.loopColor, new Color(0.9f, 0.5f, 0.961f) },
    };
}
