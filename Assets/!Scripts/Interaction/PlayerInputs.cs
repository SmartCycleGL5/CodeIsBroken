using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GhostBuilding buildingInput;
    [SerializeField] MachineInteraction machineInput;
    [SerializeField] GameObject buildingMenu;
    [SerializeField] Camera cam;

    [SerializeField] Transform player;

    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] int tiltLimit;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;

    private Vector2 moveInput;
    private Vector2 lookInput;

    public enum PlayerAction
    {
        Building,
        WorldInteraction
    }
    public PlayerAction playerAction;

    #region Player inputs
    // Player WASD movement
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    // Player mouse movement
    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
    #endregion
    
    void Update()
    {
        PlayerUpdate();
        Movement();
        MouseRotate();
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
        player.position+=(moveDirection*moveSpeed*Time.deltaTime);
    }

    void MouseRotate()
    {
        if (!Mouse.current.middleButton.IsPressed()) return;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, cam.transform.forward, out hit, 100))
        {
            player.RotateAround(hit.point, Vector3.up, lookInput.x*rotationSpeed*Time.deltaTime);
        }
    }
}
