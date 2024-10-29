using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;              
using System.Collections;           

public class PlatformElevator : MonoBehaviour
{
    [SerializeField] private float riseSpeed = 2f;        
    [SerializeField] private string nextSceneName;       
    [SerializeField] private GameObject fadePanel;       
    [SerializeField] private float fadeDuration = 3f;    
    [SerializeField] private float totalRiseTime = 10f; 

    private bool playerOnPlatform = false;
    private bool platformActivated = false;
    private Image fadePanelImage;  

    private int elevatorLayer;
    private int groundLayer;
    private int ceilingLayer;
    private int playerLayer;

    private GameObject player;
    private MovementV2 movementV2;

    private float elapsedTime = 0f;

    void Start()
    {
        // Get the layers by name
        elevatorLayer = LayerMask.NameToLayer("Elevator");
        groundLayer = LayerMask.NameToLayer("Ground");
        ceilingLayer = LayerMask.NameToLayer("Ceiling");
        playerLayer = LayerMask.NameToLayer("Player");

        fadePanelImage = fadePanel.GetComponent<Image>();
    }


    void Update()
    {
        // Activate platform on player input and start fade effect
        if (playerOnPlatform && !platformActivated)
        {
            platformActivated = true;
            StartCoroutine(FadeInAndLoadScene());

            // Disable collision between elevator, ground, ceiling, and player layers
            Physics.IgnoreLayerCollision(elevatorLayer, groundLayer, true);
            Physics.IgnoreLayerCollision(elevatorLayer, ceilingLayer, true);
            Physics.IgnoreLayerCollision(playerLayer, groundLayer, true);
            Physics.IgnoreLayerCollision(playerLayer, ceilingLayer, true);

            // Disable the player's movement script to prevent walking off the elevator
            if (movementV2 != null)
            {
                movementV2.enabled = false;
            }

        }

        // Move elevator up if activated, for a total of 10 seconds
        if (platformActivated && elapsedTime < totalRiseTime)
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;

            // Start fading during the last 3 seconds of the rise
            if (elapsedTime >= totalRiseTime - fadeDuration  && fadePanelImage.color.a < 1f)
            {
                float fadeElapsedTime = elapsedTime - (totalRiseTime - fadeDuration);
                Color fadeColor = fadePanelImage.color;
                fadeColor.a = Mathf.Clamp01(fadeElapsedTime / fadeDuration);
                fadePanelImage.color = fadeColor;
            }
        }
        else if (platformActivated && elapsedTime >= totalRiseTime)
        {
            // Re-enable collision between elevator, ground, ceiling, and player layers
            Physics.IgnoreLayerCollision(elevatorLayer, groundLayer, false);
            Physics.IgnoreLayerCollision(elevatorLayer, ceilingLayer, false);
            Physics.IgnoreLayerCollision(playerLayer, groundLayer, false);
            Physics.IgnoreLayerCollision(playerLayer, ceilingLayer, false);

            // Re-enable the player's movement script
            if (movementV2 != null)
            {
                movementV2.enabled = true;
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Inside Elevator");

        if (collider.gameObject.CompareTag("Player") && !playerOnPlatform)
        {
            Debug.Log("Player on platform");
            playerOnPlatform = true;
            player = collider.gameObject;
            movementV2 = player.GetComponent<MovementV2>();
        }
    }


    private IEnumerator FadeInAndLoadScene()
    {
        yield return new WaitForSecondsRealtime(totalRiseTime);  // Wait for the elevator to complete rising

        yield return new WaitForSecondsRealtime(0.5f); // Finish running processes

        UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }
}
