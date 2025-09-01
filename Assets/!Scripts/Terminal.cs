using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public TMP_InputField input;

    public MachineScript machine;

    public void Initialize()
    {
        machine.ClearMemory();
        Interpreter.InterperateInitialization(input.text, ref machine);
    }
}
