using UnityEngine;
using UnityEngine.SceneManagement;  // For scene transitions

public class PlatformElevator : MonoBehaviour
{
    [SerializeField]
    private float riseSpeed = 2f;      // Speed at which the platform rises
    [SerializeField]
    private string nextSceneName;      // Name of the next scene to load

    [SerializeField]
    private bool playerOnPlatform = false;
    [SerializeField]
    private bool platformActivated = false;

    void Update()
    {
        if (playerOnPlatform && Input.GetKeyDown(KeyCode.E))
        {
            platformActivated = true;
        }

        if (platformActivated && transform.position.y < 3f)  // Arbitrary height
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        }
        else if (platformActivated && transform.position.y >= 3f)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player on platform"); // Debug log
            playerOnPlatform = true;
        }
    }
}