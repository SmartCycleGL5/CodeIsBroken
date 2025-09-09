using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public static Terminal Instance;
    public TMP_InputField input;

    public MachineScript machineToEdit { get; private set; }

    private void Start()
    {
        Instance = this;
        SelectMachine(machineToEdit);
    }

    public void Initialize()
    {
        if(machineToEdit != null)
            machineToEdit.Initialize(input.text);
    }
    public void SelectMachine(MachineScript machineScript)
    {
        machineToEdit = machineScript;

        input.text = machineScript.machineCode;
    }

    public void Run()
    {
        ScriptManager.StartMachines();
    }
    public void Save()
    {
        if (machineToEdit != null)
            machineToEdit.machineCode = input.text;
    }
}
