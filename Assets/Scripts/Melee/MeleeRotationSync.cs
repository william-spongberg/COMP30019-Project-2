using UnityEngine;

public class MeleeRotationSync : MonoBehaviour
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
        // Get the camera's pitch (X-axis) rotation and invert it
        float invertedCameraPitch = -Camera.eulerAngles.x;

        // Keep the item's current Y and Z rotation, only apply the inverted camera's X-axis (pitch)
        transform.localRotation = Quaternion.Euler(invertedCameraPitch, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
    }
}
