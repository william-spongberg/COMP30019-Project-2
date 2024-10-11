using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementV2 : MonoBehaviour
{
    private Rigidbody rb;

    // Movement settings
    private float moveSpeed;
    [SerializeField]
    private float walkSpeed = 5f;
    [SerializeField]
    private float sprintSpeed = 5f;
    [SerializeField]
    private float jumpForce = 10f;
    [SerializeField]
    private float gravity = -9.8f;

    // For checking grounded state
    private bool isGrounded;
    [SerializeField]
    private float modelHeight;
    [SerializeField]
    private LayerMask ground;

    private float horizontalInput;
    private float verticalInput;
    private bool jumpInput;
    private bool sprintInput;

    // Slope Handling
    [SerializeField] private float maxSlopeAngle = 45f;  // Max slope player can climb
    private RaycastHit slopeHit;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Update() {

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Sprint input
        sprintInput = Input.GetKey(KeyCode.LeftShift);

        // Set speed based on sprinting
        if (sprintInput) 
        {
            moveSpeed = sprintSpeed;
        }
        else 
        {
            moveSpeed = walkSpeed;
        }
        
        // Only set jumpInput to true when space is pressed
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            jumpInput = true;  
        }

        Debug.DrawRay(transform.position, Vector3.down * (modelHeight * 0.5f + 0.1f), Color.red);

    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement() 
    {

        // Calculate if player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, modelHeight * 0.5f + 0.1f, ground);
        // Use input to determine Horizontal Movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        
        // transform movement to align with current orientation of player
        moveDirection = transform.TransformDirection(moveDirection);

        // Check for slopes and adjust movement
        if (OnSlope())
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);  // Adjust for slopes
        }

        Vector3 move = moveDirection * moveSpeed;

        // Use Rigidbody to Move the Character
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);

        // Handle Jump (if grounded)
        if (isGrounded && jumpInput)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            jumpInput = false;  // Reset jump
        }

        // Manual Gravity
        if (!isGrounded)
        {
            rb.velocity += Vector3.up * gravity * Time.fixedDeltaTime;
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, modelHeight * 0.5f + 0.2f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle;
        }
        return false;
    }

}
