using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System.Threading.Tasks;
using UnityEngine;
using Terminal.Language;

[DefaultExecutionOrder(100)]
public class MachineScript : MonoBehaviour
{
    [ResizableTextArea] public string machineCode;

    [SerializedDictionary("Name", "Class")]
    public SerializedDictionary<string, Class> Classes;

    MachineScript machine;

    private void Start()
    {
        machine = this;
        Initialize(machineCode);
        ScriptManager.instance.AddMachine(this);
    }
    private void OnDestroy()
    {
        ScriptManager.instance.RemoveMachine(this);
    }

    public void Run()
    {
        foreach (var Class in Classes)
        {
            Class.Value.TryRunMethod("Start()");
        }
    }

    public void ClearMemory()
    {
        Classes.Clear();
    }
    public void ResetThis()
    {
        transform.position = new Vector3(6, 0, 0);
        transform.eulerAngles = Vector3.zero;
    }
    public void Initialize(string raw)
    {
        machineCode = raw;
        Interpreter.InterperateInitialization(machineCode, ref machine);
    }


    public async Task Rotate(int amount)
    {
        float originalAmount = transform.eulerAngles.y;

        while ((originalAmount + amount) - transform.eulerAngles.y > .1f)
        {
            transform.Rotate(0, amount * Time.deltaTime, 0);

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        transform.eulerAngles = new Vector3(0, originalAmount + amount, 0);
    }
    public async Task Move(Vector3 dir)
    {
        Vector3 originalPos = transform.position;

        while (Vector3.Distance(originalPos + dir, transform.position) > .1f)
        {
            Debug.Log(Vector3.Distance(originalPos + dir, transform.position));

            transform.position += dir * Time.deltaTime;

            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        transform.position = originalPos + dir;
    }

    public async void Rocket()
    {
        while (true)
        {
            _ = Rotate(360);
            await Move(Vector3.up);
        }
    }
}
