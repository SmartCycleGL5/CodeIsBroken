using Coding;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GhostBuilding buildingInput;
    [SerializeField] MachineInteraction machineInput;
    [SerializeField] GameObject buildingMenu;
    [SerializeField] Camera cam;
    [SerializeField] private TMP_InputField terminal;

    [SerializeField] Transform player;

    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] int tiltLimit;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;

    [Header("Player Options")]
    [SerializeField] bool dragToRotate;

    bool isBuilding;
    private Vector2 moveInput;
    private float lookInput;

    float screenWidth;
    public enum PlayerAction
    {
        Building,
        WorldInteraction,
        TerminalInteraction
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
        //lookInput = value.Get<Vector2>();
    }
    public void OnCameraRotate(InputValue value)
    {
        //Debug.Log(value.Get<Vector2>());
        lookInput = value.Get<float>();
    }
    #endregion

    private void Start()
    {
        playerAction = PlayerAction.WorldInteraction;
        screenWidth = Screen.width;
    }

    void Update()
    {
        PlayerUpdate();
        if (Terminal.focused) return;
        Movement();
        MouseRotate();
    }


    void PlayerUpdate()
    {
        //Enable disable building:
        if (Keyboard.current.bKey.wasPressedThisFrame && !isBuilding)
        {
            if (Terminal.focused) return;
            isBuilding = true;
            buildingMenu.SetActive(true);
            playerAction = PlayerAction.Building;
            Debug.Log("Enabled Building");
        }
        else if (Keyboard.current.bKey.wasPressedThisFrame && isBuilding || Keyboard.current.escapeKey.wasPressedThisFrame && isBuilding)
        {
            playerAction = PlayerAction.WorldInteraction;
            isBuilding = false;
            buildingMenu.SetActive(false);
            buildingInput.DestroyGhost();
            Debug.Log("Disabled Building");
        }
        if (Keyboard.current.deleteKey.wasPressedThisFrame)
        {
            Restart();
        }

        // Updates The active scripts
        if (playerAction == PlayerAction.Building)
        {
            buildingInput.PlayerUpdate();
        }
        if (playerAction == PlayerAction.WorldInteraction)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame) { 
                WindowManager.CloseAllWindows();
            }
            machineInput.PlayerUpdate();
        }
    }


    void Movement()
    {
        Vector3 moveDirection = player.forward * moveInput.y + player.right * moveInput.x;
        player.position+=(moveDirection*moveSpeed*Time.deltaTime);
    }

    void MouseRotate()
    {
        // Only rotate if Middle mouse i down
        //if (!Mouse.current.middleButton.IsPressed()) return;
        RaycastHit hit;
        if(Physics.Raycast(transform.position, cam.transform.forward, out hit, 100))
        {
            // Sideways rotation
            //Drag to rotate, rotates based on mouse movement.
            if (dragToRotate)
            {
                //player.RotateAround(hit.point, Vector3.up, lookInput.x * rotationSpeed * Time.deltaTime);
            }
            //Rotates based on which side of the screen mouse is in.
            else
            {
                //Vector2 mousePosition = Mouse.current.position.value;
                //float screenSide = mousePosition.x < screenWidth / 2f ? -5 : 5;
                //Debug.Log("Rotating"+lookInput);
                player.RotateAround(hit.point, Vector3.up, -lookInput * rotationSpeed * Time.deltaTime);
            }
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
