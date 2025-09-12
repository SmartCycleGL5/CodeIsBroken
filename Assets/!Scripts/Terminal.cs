using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Terminal
{
public class Terminal : MonoBehaviour
{
    public static Terminal Instance;
    public TMP_InputField input;

    [field: SerializeField]public MachineScript machineToEdit { get; private set; }

    private void Start()
    {
        Instance = this;
        SelectMachine(machineToEdit);
    }
    public void SelectMachine(MachineScript machineScript)
    {
        machineToEdit = machineScript;

        Load();
    }

    [Button]
    public void Load()
    {
        if (machineToEdit != null)
            input.text = machineToEdit.machineCode.Code;
    }
    [Button]
    public void Save()
    {
        if (machineToEdit != null)
            machineToEdit.machineCode.Code = input.text;
    }
}
}
