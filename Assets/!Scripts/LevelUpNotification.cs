using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelUpNotification : MonoBehaviour
{
    [SerializeField] RectTransform panelObj;
    [SerializeField] TextMeshProUGUI notification;
    private void Start()
    {
        Invoke("Notify", 2);
    }

    void Notify()
    {
        panelObj.DOJumpAnchorPos(new Vector2(0, 1000),3f, 1,0.1f);
    }
}
