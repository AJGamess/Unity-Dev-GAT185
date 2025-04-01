using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerData data;
    [SerializeField] Transform view;

    [SerializeField] Animator animator;

    [SerializeField] StringEvent loadLevelEvent;

    CharacterController controller;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;
    InputAction attackAction;

    bool ifSprint = false;

    Vector2 movementInput = Vector2.zero;
    Vector3 velocity = Vector3.zero;

    public Transform View { get => view; set => view = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        jumpAction = InputSystem.actions.FindAction("Jump");
        jumpAction.performed += OnJump;
        jumpAction.canceled += OnJump;

        sprintAction = InputSystem.actions.FindAction("Sprint");
        sprintAction.performed += OnSprint;
        sprintAction.canceled += OnSprint;

        attackAction = InputSystem.actions.FindAction("Attack");
        attackAction.performed += OnAttack;
        attackAction.canceled += OnAttack;

        controller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Check if the player is on ground
        bool onGround = controller.isGrounded; // || (velocity.y < 0 && PredictGroundContact());

        // Reset vertical velocity when grounded to prevent accumulating downward force
        if (onGround && velocity.y < 0)
        {
            velocity.y = -1; // Small downward force to keep player grounded
        }

        // Convert movement input into a world-space direction based on the player's view rotation
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y);
        movement = Quaternion.AngleAxis(view.rotation.eulerAngles.y, Vector3.up) * movement;

        // Initialize acceleration vector for movement calculations
        Vector3 acceleration = Vector3.zero;
        acceleration.x = movement.x * data.acceleration;
        acceleration.z = movement.z * data.acceleration;

        // Reduce acceleration while in the air for smoother movement control
        if (!onGround) acceleration *= 0.1f;

        // Extract horizontal velocity (ignoring vertical movement)
        Vector3 vXZ = new Vector3(velocity.x, 0, velocity.z);

        // Apply acceleration to velocity while limiting max speed
        vXZ += acceleration * Time.deltaTime;
        vXZ = Vector3.ClampMagnitude(vXZ, (ifSprint) ? data.sprintSpeed : data.speed);

        // Assign updated velocity values
        velocity.x = vXZ.x;
        velocity.z = vXZ.z;

        // Apply drag to slow the player down when there is no input or when airborne
        if (movement.sqrMagnitude <= 0 || !onGround)
        {
            float drag = (onGround) ? 10 : 4;
            velocity.x = Mathf.MoveTowards(velocity.x, 0, drag * Time.deltaTime);
            velocity.z = Mathf.MoveTowards(velocity.z, 0, drag * Time.deltaTime);
        }

        // Smoothly rotate the player towards the movement direction
        if (movement.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), Time.deltaTime * data.turnRate);
        }

        // Apply gravity
        velocity.y += data.gravity * Time.deltaTime;

        // Move the character using the CharacterController component
        controller.Move(velocity * Time.deltaTime);

        // Update animator parameters for movement animations
        animator.SetFloat("Speed", new Vector3(velocity.x, 0, velocity.z).magnitude);
        animator.SetFloat("AirSpeed", controller.velocity.y);
        animator.SetBool("OnGround", onGround);

        if (Input.GetKeyDown(KeyCode.L))
        {
            loadLevelEvent?.RaiseEvent("Character");
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed && controller.isGrounded)
        {
            
            velocity.y = Mathf.Sqrt(-2 * data.gravity * data.jumpHeight);
            animator.SetTrigger("Jump");

        }
    }
    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            animator.SetTrigger("Attack");
        }
    }
    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed) ifSprint = true;
        else if (ctx.phase == InputActionPhase.Canceled) ifSprint = false;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var rb = hit.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic || hit.moveDirection.y < -0.3f) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        rb.linearVelocity = pushDir * data.pushForce;


    }
}
