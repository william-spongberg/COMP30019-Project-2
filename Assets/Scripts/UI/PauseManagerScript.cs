using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManagerScript : MonoBehaviour
{
    // Reference to buttons
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button settingsMenuButton;
    [SerializeField] private Button instructionsMenuButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    // Reference to other UI pages
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject instructionsOne;
    [SerializeField] private GameObject instructionsTwo;
    [SerializeField] private GameObject crossHair;

    public static bool IsPaused { get; set; } = false;


    void Start()
    {
        // Start game unpaused, cursor locked
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        OpenPauseMenu();
        crossHair.SetActive(false);
        Time.timeScale = 0f;
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        ClosePauseMenu();
        crossHair.SetActive(true);
        Time.timeScale = 1f;
        IsPaused = false;

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
        ClosePauseMenu();
        settingsPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        OpenPauseMenu();
    }

    public void OpenInstructionsPageOne()
    {
        ClosePauseMenu();
        instructionsPanel.SetActive(true);
        instructionsOne.SetActive(true);
        nextButton.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

     public void OpenInstructionsPageTwo()
    {

        instructionsOne.SetActive(false);
        nextButton.gameObject.SetActive(false);

        instructionsPanel.SetActive(true);
        instructionsTwo.SetActive(true);
        backButton.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseInstructions()
    {   
        instructionsPanel.SetActive(false);
        instructionsTwo.SetActive(false);
        backButton.gameObject.SetActive(false);
        OpenPauseMenu();
    }

    private void OpenPauseMenu()
    {
        mainMenuButton.gameObject.SetActive(true);
        resumeButton.gameObject.SetActive(true);
        settingsMenuButton.gameObject.SetActive(true);
        instructionsMenuButton.gameObject.SetActive(true);
    }

    private void ClosePauseMenu()
    {
        mainMenuButton.gameObject.SetActive(false);
        resumeButton.gameObject.SetActive(false);
        settingsMenuButton.gameObject.SetActive(false);
        instructionsMenuButton.gameObject.SetActive(false);
    }
}
