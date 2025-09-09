using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class MachineScript : MonoBehaviour
{
    [SerializedDictionary("Name", "Class")]
    public SerializedDictionary<string, Class> Classes;

    static MachineScript instance;

    private void Start()
    {
        instance = this;
    }

    public void ClearMemory()
    {
        Classes.Clear();
    }
    public static void Reset()
    {
        instance.transform.position = new Vector3(6, 0, 0);
        instance.transform.eulerAngles = Vector3.zero;
    }
    public static void Rotate(int amount)
    {
        instance.transform.Rotate(0, amount, 0);
    }
    public static void Move(Vector3 dir)
    {
        instance.transform.position += dir;
    }
}
