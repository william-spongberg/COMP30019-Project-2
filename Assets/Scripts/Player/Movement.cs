using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public float groundFriction;
    public float modelHeight;
    public LayerMask ground;
    bool grounded; 

    public Transform orientation;

    public float jumpForce;
    public float jumpDelay;
    public float airForce;
    public KeyCode jumpKey = KeyCode.Space;
    bool canJump = true;

    public float dashForce;
    public float dashDelay;
    public KeyCode dashKey = KeyCode.Mouse1;
    bool canDash = true;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, modelHeight * 0.5f + 0.1f, ground);
        MoveInput();

        SpeedLimit();
        rb.drag = grounded ? groundFriction : 0;
    }

    private void MoveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpDelay);
        }

        if (Input.GetKey(dashKey) && canDash && grounded)
        {
            canDash = false;
            Dash();
            Invoke(nameof(ResetDash), dashDelay);
        }
    }
    
    private void FixedUpdate()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 horizontalMoveDirection = new Vector3(moveDirection.x, 0, moveDirection.z).normalized;

        if (grounded)
        {
            rb.AddForce(horizontalMoveDirection * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(horizontalMoveDirection * moveSpeed * 10f * airForce, ForceMode.Force);
        }
    }

    private void SpeedLimit()
    {
        Vector3 currentVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (currentVelocity.magnitude > moveSpeed)
        {
            Vector3 maxVelocity = currentVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(maxVelocity.x, rb.velocity.y, maxVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void Dash()
    {
        rb.AddForce(orientation.forward * dashForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private void ResetDash()
    {
        canDash = true;
    }
}