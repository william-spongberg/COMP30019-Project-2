using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementV2 : MonoBehaviour
{
    private Rigidbody rb;

    // Movement settings
    [SerializeField]
    private float moveSpeed = 5f;
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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

    }

    private void Update() {

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
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
        // Use input to determine Horizontal Movement direction
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;
        
        // transform movement to align with current orientation of player
        Vector3 move = transform.TransformDirection(moveDirection) * moveSpeed;

        // Use Rigidbody to Move the Character
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);

        // Calculate if player is on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, modelHeight * 0.5f + 0.1f, ground);

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
}
