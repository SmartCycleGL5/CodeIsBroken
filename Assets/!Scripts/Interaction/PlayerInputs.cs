using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GhostBuilding buildingInput;
    [SerializeField] MachineInteraction machineInput;
    [SerializeField] GameObject buildingMenu;
    [SerializeField] InputAction inputAction;

    [SerializeField] Transform player;

    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] int xTiltLimit;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;

    private Vector2 moveInput;

    public enum PlayerAction
    {
        Building,
        WorldInteraction
    }

    public PlayerAction playerAction;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerUpdate();
        Movement();
    }


    void PlayerUpdate()
    {
        // Updates The active scripts
        if (playerAction == PlayerAction.Building)
        {
            buildingInput.PlayerUpdate();
        }
        if (playerAction == PlayerAction.WorldInteraction)
        {
            machineInput.PlayerUpdate();
        }

        //Enable disable building:
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            buildingMenu.SetActive(false);
            buildingInput.DestroyGhost();
            playerAction = PlayerAction.WorldInteraction;
        }
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            buildingMenu.SetActive(true);
            playerAction = PlayerAction.Building;
        }
    }


    void Movement()
    {
        Vector3 moveDirection = player.forward * moveInput.y + player.right * moveInput.x;
        transform.Translate(moveDirection*moveSpeed*Time.deltaTime);
    }
}
