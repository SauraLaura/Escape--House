using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    [Header("MOVEMENT")]
    [SerializeField] float walkSpeed = 5f;
    [SerializeField] float sprintDuration = 5f;
    [SerializeField] float maxSprintDuration = 5f;
    CharacterController controller;

    enum MovementState
    {
        Walking,
        Running,
        Idle
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

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

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
            if (Input.GetKey(KeyCode.LeftShift))
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
                sprintDuration += 1.5f * Time.deltaTime;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
