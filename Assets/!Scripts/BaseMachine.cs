using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Threading.Tasks;
using UnityEngine;
using Coding.Language;
using Coding;
using Unity.VisualScripting;

[DefaultExecutionOrder(100), DisallowMultipleComponent]
public class BaseMachine : MonoBehaviour
{
    public MachineCode machineCode;

    [SerializedDictionary("Name", "Class")]
    public SerializedDictionary<string, Class> Classes;

    public bool isRunning { get; private set; } = false;

    Vector3 initialPos;
    Vector3 initialRot;

    private void Start()
    {
        machineCode.Initialize(name, this);
        ScriptManager.instance.AddMachine(this);

        initialPos = transform.position;
        initialRot = transform.eulerAngles;

        if(machineCode == null)
            machineCode = new MachineCode();
    }

    private void OnDestroy()
    {
        ScriptManager.instance.RemoveMachine(this);
    }

    public void Run()
    {
        isRunning = true;

        Application.quitting += Stop;

        foreach (var Class in Classes)
        {
            Class.Value.TryRunMethod("Start()");
        }
    }
    public void Stop()
    {
        Application.quitting -= Stop;
        isRunning = false;

        ResetThis();
    }

    public void ClearMemory()
    {
        Classes.Clear();
    }


    public void ResetThis()
    {
        transform.position = initialPos;
        transform.eulerAngles = initialRot;
    }
    public async Task Rotate(int amount)
    {
        float originalAmount = transform.eulerAngles.y;

        while ((originalAmount + amount) - transform.eulerAngles.y > .1f && isRunning)
        {
            transform.Rotate(0, amount * Time.deltaTime, 0);

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        if(isRunning)
            transform.eulerAngles = new Vector3(0, originalAmount + amount, 0);
    }
    public async Task Move(Vector3 dir)
    {
        Vector3 originalPos = transform.position;

        while (Vector3.Distance(originalPos + dir, transform.position) > .1f && isRunning)
        {
            transform.position += dir * Time.deltaTime;

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        if (isRunning)
            transform.position = originalPos + dir;
    }

    public async void Rocket()
    {
        while (isRunning)
        {
            _ = Rotate(360);
            await Move(Vector3.up);

            Debug.Log(isRunning);
        }
    }

    [Button]
    public void OpenTerminalForMachine()
    {
        Terminal.NewTerminal(this);
    }
}
