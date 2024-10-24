

using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;              
using System.Collections;           

public class PlatformElevator : MonoBehaviour
{
    [SerializeField]
    private float riseSpeed = 2f;      // Speed at which the platform rises
    [SerializeField]
    private string nextSceneName;      // Name of the next scene to load

    [SerializeField]
    private GameObject fadeImagePrefab;  // Prefab for the fade Image
    [SerializeField]
    private float fadeDuration = 1f;   // Duration of the fade effect

    private bool playerOnPlatform = false;
    private bool platformActivated = false;
    private Image fadeImage;           // Reference to the instantiated fade Image

    void Update()
    {
        if (playerOnPlatform && Input.GetKeyDown(KeyCode.E))
        {
            platformActivated = true;
            InstantiateFadeImage();    // Instantiate fade image prefab
            StartCoroutine(FadeIn());  // Start fading in when the platform is activated
        }

        if (platformActivated && transform.position.y < 3f)  // Arbitrary height
        {
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);
        }
        else if (platformActivated && transform.position.y >= 3f)
        {
            SceneManager.LoadScene(nextSceneName);  // Load the next scene immediately after reaching the height
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

    private void InstantiateFadeImage()
    {
        // Instantiate the fade image prefab and set it up
        GameObject fadeObject = Instantiate(fadeImagePrefab, transform.position, Quaternion.identity);
        fadeObject.transform.SetParent(GameObject.Find("Canvas").transform, false);

        fadeImage = fadeObject.GetComponent<Image>();  

        if (fadeImage != null)
        {
            Color fadeColor = fadeImage.color;
            fadeColor.a = 0f; 
            fadeImage.color = fadeColor;
        }
        else
        {
            Debug.LogError("Fade Image component not found on prefab!");
        }
    }

   private IEnumerator FadeIn()
{
    float elapsedTime = 0f;
    Color fadeColor = fadeImage.color;

   
    float fastFadeDuration = 1.5f; 


    while (elapsedTime < fastFadeDuration)
    {
        elapsedTime += Time.deltaTime;
        fadeColor.a = Mathf.Clamp01(elapsedTime / fastFadeDuration);
        fadeImage.color = fadeColor;
        yield return null;
    }

   
    fadeColor.a = 1f;
    fadeImage.color = fadeColor;
}

}
