using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            menu.SetActive(!menu.activeSelf);
        }
    }

    public void Continue()
    {
        menu.SetActive(false);
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
