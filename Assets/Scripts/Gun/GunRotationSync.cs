using UnityEngine;

public class GunRotationSync : MonoBehaviour
{
    [SerializeField]
    private Transform Camera;  // Reference to the main camera

    void Start()
    {
        // Locks cursor to the game window
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // Get the camera's pitch (X-axis) rotation
        float cameraPitch = Camera.eulerAngles.x;

        // Keep the gun's current Y and Z rotation, only apply the camera's X-axis (pitch)
        transform.localRotation = Quaternion.Euler(cameraPitch, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}
