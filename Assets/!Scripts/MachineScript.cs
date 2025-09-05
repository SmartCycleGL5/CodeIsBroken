using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

public class MachineScript : MonoBehaviour
{
    [SerializedDictionary("Name", "Class")]
    public SerializedDictionary<string, Class> Classes;

    private void Start()
    {
        
    }

    public void ClearMemory()
    {
        Classes.Clear();
    }
}
