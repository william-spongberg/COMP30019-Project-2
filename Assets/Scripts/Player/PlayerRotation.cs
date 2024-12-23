using System;
using UnityEngine;

public class PlayerRotationSync : MonoBehaviour
{
    [SerializeField]
    private Transform Camera;  // Reference

    private Rigidbody rb;

    private Boolean canRotate = true;

    void Start()
    {
        // Locks cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
    }
    
    void FixedUpdate() // FixedUpdate since using physics
    {
        if (!canRotate) return;

        // Sync the player's Y-axis rotation with the camera's Y-axis rotation (yaw)
        Vector3 cameraRotation = Camera.eulerAngles;
        Quaternion newRotation = Quaternion.Euler(0f, cameraRotation.y, 0f);
        
        // Apply the rotation to the rigid body
        rb.MoveRotation(newRotation);
    }

    public void DisableRotation()
    {
        canRotate = false;
    }
}
