using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to Pause Menu UI
    public GameObject playerCapsule;  // Reference to the PlayerCapsule (which has the ShootingGun script attached)

    private ShootingGun shootingGunScript;  // Reference to the ShootingGun script

    public static bool isPaused = false;

    void Start()
    {
       
        shootingGunScript = playerCapsule.GetComponent<ShootingGun>();

      
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the P key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    void Pause()
    {
      
        pauseMenuUI.SetActive(true);   
        Time.timeScale = 0f;          
        isPaused = true;

       
        if (shootingGunScript != null)
        {
            shootingGunScript.enabled = false;
        }

     
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        
        pauseMenuUI.SetActive(false);  
        Time.timeScale = 1f;         
        isPaused = false;

       
        if (shootingGunScript != null)
        {
            shootingGunScript.enabled = true;
        }

      
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadMainMenu()
    {

        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

    // public void QuitGame()
    // {
    //     Debug.Log("Quitting game...");
    //     Application.Quit();
    // }
}

