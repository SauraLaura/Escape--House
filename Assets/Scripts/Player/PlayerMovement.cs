using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [Header("MOVEMENT")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintDuration = 3f;
    [SerializeField] float maxSprintDuration = 3f;
    CharacterController controller;

    InputSystem_Actions inputActions;
    InputAction moveAction;
    InputAction sprintAction;

    enum MovementState
    {
        Walking,
        Running,
        Idle,
        Stopped
    }

    [SerializeField] MovementState currentState;

    [Header("Gravity")]
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.4f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float gravity = -9.81f;
    Vector3 velocity;
    Vector3 move;

    [Header("UI")]
    [SerializeField] Image staminaBar;


    public static PlayerMovement instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Enable();
        moveAction = inputActions.Player.Move;
        sprintAction = inputActions.Player.Sprint;
    }
    void Update()
    {
        Vector2 inputVector = moveAction.ReadValue<Vector2>();
        float inputX = inputVector.x;
        float inputZ = inputVector.y;

        //Movement 
        move = transform.right * inputX + transform.forward * inputZ; //Get the direction based on input
        move.Normalize(); // Normalize to prevent faster diagonal movement
        StateSwitcher();


        //Gravity logic
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        ResetVelocity();
    }

    #region State Logic
    void StateSwitcher()
    {
        sprintDuration = Mathf.Clamp(sprintDuration, 0, maxSprintDuration);
        if (move.magnitude > 0.1f)
        {
            if (sprintAction.IsPressed())
            {
                sprintDuration -= Time.deltaTime;
                if (sprintDuration > 0f)
                {
                    currentState = MovementState.Running;
                }
                else
                {
                    currentState = MovementState.Walking;
                }
            }
            else
            {
                currentState = MovementState.Walking;
            }
            if(DialogueManager.instance != null && DialogueManager.instance.dialogueActive)
            {
                currentState = MovementState.Stopped;
            }
        }
        else
        {
            currentState = MovementState.Idle;
        }
        MoveStateManager();
        UpdateStaminaUI();
    }

    void MoveStateManager()
    {
        switch (currentState)
        {
            case MovementState.Walking:
                controller.Move(move * walkSpeed * Time.deltaTime);
                sprintDuration += 1.0f * Time.deltaTime;
                break;
            case MovementState.Running:
                controller.Move(move * walkSpeed * 2 * Time.deltaTime);
                break;
            case MovementState.Idle:
                sprintDuration += 2f * Time.deltaTime;
                break;
            case MovementState.Stopped:
                controller.Move(Vector3.zero);
                break;
        }
    }

    void UpdateStaminaUI()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = sprintDuration / maxSprintDuration;
        }
    }
    #endregion State Logic

    #region Gravity Logic
    void ResetVelocity()
    {
        if (IsGrounded() && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
    }
    #endregion Gravity Logic

    void OnDisable()
    {
        inputActions.Disable();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
