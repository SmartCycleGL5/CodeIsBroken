using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    private void Start()
    {
        Instance = this;
    }
}
