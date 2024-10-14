using UnityEngine;
using UnityEngine.SceneManagement;  // For scene transitions

public class PlatformElevator : MonoBehaviour
{
    public float riseSpeed = 2f;      // Speed at which the platform rises
    public string nextSceneName;      // Name of the next scene to load

    private bool playerOnPlatform = false;
    private bool platformActivated = false;
    private Transform playerTransform;  // Reference to the player's transform

    void Update()
    {
       
        if (playerOnPlatform && Input.GetKeyDown(KeyCode.E))
        {
            platformActivated = true;
        }

        if (platformActivated && transform.position.y < 10f)  // Arbitrary height
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        }
        else if (platformActivated && transform.position.y >= 10f)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = true;
            playerTransform = other.transform;  // Store the player's transform
            playerTransform.SetParent(transform);  // Make the player a child of the platform
        }
    }

    // Detect when the player leaves the platform
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = false;
            playerTransform.SetParent(null);  // Unparent the player when they leave the platform
            playerTransform = null;  // Clear the reference
        }
    }
}
