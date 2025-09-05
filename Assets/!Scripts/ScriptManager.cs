using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public static Class UniversalClass { get { return scriptManager._UniversalClass; } }
    public Class _UniversalClass;

    public static ScriptManager scriptManager;

    private void Start()
    {
        scriptManager = this;
    }
}
