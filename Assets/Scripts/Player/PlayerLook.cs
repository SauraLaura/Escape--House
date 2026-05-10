using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] public float mouseSens = 150f;
    [SerializeField] Transform playerBody;

    InputSystem_Actions inputActions;
    InputAction lookAction;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
    }

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        lookAction = inputActions.Player.Look;
    }

    void Update()
    {
        Vector2 lookVector = lookAction.ReadValue<Vector2>();
        float lookX = lookVector.x * mouseSens * Time.deltaTime;

        playerBody.Rotate(Vector3.up * lookX);//To rotate the parent object which is the player.
    }

    void OnDisable()
    {
        inputActions.Disable();
    }
}
