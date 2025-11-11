using DG.Tweening;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class LevelUpNotification : MonoBehaviour
{
    [SerializeField] RectTransform panelObj;
    [SerializeField] TextMeshProUGUI notification;
    private void Start()
    {
        Invoke(nameof(NotifyMessage), 2);
        PlayerProgression.onLevelUp += NotifyLevelUp;
    }

    void NotifyMessage()
    {
        notification.text = "Deliver any item into the seller!";
        panelObj.DOJumpAnchorPos(new Vector2(0, 650),3f, 1,0.3f);
        Invoke("HideUI", 9);
    }
    
    void NotifyLevelUp(int level)
    {
        int timeFromStart = (int)Time.time;
        notification.text = $"Good job! You reached level {level} in {timeFromStart} seconds. Press J to check journal for new machines!";
        panelObj.DOJumpAnchorPos(new Vector2(0, 650),3f, 1,0.3f);
        Invoke("HideUI", 9);
    }

    void HideUI() 
    {
        panelObj.DOJumpAnchorPos(new Vector2(0, 1000),3f, 1,0.2f);
    }
}
