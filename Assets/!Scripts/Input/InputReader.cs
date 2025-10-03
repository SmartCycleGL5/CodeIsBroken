using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    [HideInInspector] public PlayerInput playerInput;

    private void OnEnable()
    {
        playerInput = new PlayerInput();
        playerInput.Player.SetCallbacks(this);

        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }

    public void OnOpenJournal(InputAction.CallbackContext context)
    {
        Debug.Log("Open Journal");
        if (!context.started) return;
        UIManager.Instance.gameObject.AddComponent<ShitJournal>();
    }
}