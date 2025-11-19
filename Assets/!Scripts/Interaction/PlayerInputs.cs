using CodeIsBroken.UI.Window;
using ScriptEditor;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerInputs : MonoBehaviour
{
    public static PlayerInputs instance;

    [Header("References")]
    [SerializeField] GhostBuilding buildingInput;
    [SerializeField] MachineInteraction machineInput;
    [SerializeField] Camera cam;
    [SerializeField] private TMP_InputField terminal;

    [SerializeField] Transform player;

    [SerializeField]
    UIDocument uiDoc;

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

    private VisualElement buildingBinds;
    private VisualElement binds;

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

    private void Awake()
    {
        instance = this;
        playerAction = PlayerAction.WorldInteraction;
        screenWidth = Screen.width;
        
        binds = uiDoc.rootVisualElement.Q<VisualElement>("Keybinds");
        buildingBinds = uiDoc.rootVisualElement.Q<VisualElement>("BuildingKeybinds");
        Debug.Log("Binds" +binds);
        
    }

    void Update()
    {
        PlayerUpdate();
        if (Terminal.focused) return;

        Movement();
        MouseRotate();
    }

    public void BuildingMenu(bool isBuilding)
    {
        if (isBuilding)
        {
            if (Terminal.focused) return;
            isBuilding = true;
            playerAction = PlayerAction.Building;
            Debug.Log("Enabled Building");
        }
        else
        {
            playerAction = PlayerAction.WorldInteraction;
            isBuilding = false;
            buildingInput.DestroyGhost();
            Debug.Log("Disabled Building");
        }
    }

    void PlayerUpdate()
    {

        // Updates The active scripts
        if (playerAction == PlayerAction.Building)
        {
            buildingInput.PlayerUpdate();
            buildingBinds.style.display = DisplayStyle.Flex;
            binds.style.display = DisplayStyle.None;
        }
        if (playerAction == PlayerAction.WorldInteraction)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                WindowManager.CloseAllWindows();
            }
            binds.style.display = DisplayStyle.Flex;
            buildingBinds.style.display = DisplayStyle.None;
            machineInput.PlayerUpdate();
        }
    }


    void Movement()
    {
        Vector3 moveDirection = player.forward * moveInput.y + player.right * moveInput.x;
        player.position += (moveDirection * moveSpeed * Time.deltaTime);
    }

    void MouseRotate()
    {
        // Only rotate if Middle mouse i down
        //if (!Mouse.current.middleButton.IsPressed()) return;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, cam.transform.forward, out hit, 100))
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
