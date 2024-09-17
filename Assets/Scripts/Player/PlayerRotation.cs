using UnityEngine;

public class PlayerRotationSync : MonoBehaviour
{
    [SerializeField]
    private Transform Camera;  // Reference

    void Start()
    {
        // Locks cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // Sync the player's Y-axis rotation with the camera's Y-axis rotation (yaw)
        Vector3 cameraRotation = Camera.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, cameraRotation.y, 0f);
    }
}
