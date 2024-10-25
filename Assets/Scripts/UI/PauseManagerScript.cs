using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;  // Reference to Pause Menu UI
    public GameObject playerCapsule;  // Reference to the PlayerCapsule (which has the ShootingGun script attached)

    // private ShootingGun shootingGunScript;  // Reference to the ShootingGun script, redundant; script no longer exists

    public static bool isPaused = false;

    [SerializeField] private GameObject settingsMenuUI;

    void Start()
    {

      
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Check if the key is pressed
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (!isPaused)
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
     
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume1()
    {
       pauseMenuUI.SetActive(false);  
          Time.timeScale = 1f;
          isPaused = false;
      
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LoadMainMenu()
    {
      

        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }

   
    public void OpenSettings()
    {
        Time.timeScale = 0;
        pauseMenuUI.SetActive(false);  
        settingsMenuUI.SetActive(true); 

         // Unlock and show cursor for settings
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    
    public void CloseSettings()
    {
        settingsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true); 
    }

}

