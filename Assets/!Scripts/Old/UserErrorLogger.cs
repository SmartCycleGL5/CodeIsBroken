using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UserErrorLogger : MonoBehaviour
{
    [SerializeField] private GameObject parentPanel;
    [SerializeField] private Image errorImg;
    [SerializeField] private Image warningImg;
    [SerializeField] private TextMeshProUGUI  errorText;
    private float timer;
    

    private void Update()
    {
        timer+=Time.deltaTime;
        if(timer>1) RemoveDebug();
    }

    public void DisplayError(string msg)
    {
        timer = 0;
        Debug.LogError(msg);
        errorText.text = msg;
        parentPanel.SetActive(true);
        errorImg.gameObject.SetActive(true);
    }

    public void DisplayWarning(string msg)
    {
        timer = 0;
        Debug.LogError(msg);
        errorText.text = msg;
        parentPanel.SetActive(true);
        warningImg.gameObject.SetActive(true);
    }

    private void RemoveDebug()
    {
        parentPanel.SetActive(false);
        errorImg.gameObject.SetActive(false);
        warningImg.gameObject.SetActive(false);
    }
    
}
