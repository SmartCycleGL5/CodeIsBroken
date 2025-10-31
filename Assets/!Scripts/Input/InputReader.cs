using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Journal;

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
        if (!context.started) return;
        //UIManager.Instance.gameObject.AddComponent<ShitJournal>();
        JournalManager.instance.JournalOnOff();
    }
}