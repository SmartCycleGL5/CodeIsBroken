using AYellowpaper.SerializedCollections;
using UnityEngine;

public class MachineScript : MonoBehaviour
{
    [SerializedDictionary("Name", "Nlass")]
    public SerializedDictionary<string, Class> Classes;

    private void Start()
    {
        
    }

    public void ClearMemory()
    {
        Classes.Clear();
    }
}
