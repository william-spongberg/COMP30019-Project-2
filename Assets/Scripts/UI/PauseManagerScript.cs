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

    [SerializeField] private GameObject dialogue;

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
        CloseDialogue();
        OpenPauseMenu();
        crossHair.SetActive(false);
        Time.timeScale = 0f;
        IsPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        OpenDialogue();
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
        CloseDialogue();
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

    public void OpenDialogue()
    {
        dialogue.SetActive(true);
    }

    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    public void OpenInstructionsPageOne()
    {
        CloseDialogue();
        ClosePauseMenu();
        instructionsPanel.SetActive(true);
        instructionsOne.SetActive(true);
        nextButton.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

     public void OpenInstructionsPageTwo()
    {
        CloseDialogue();
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
        CloseDialogue();
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

    public bool currentlyPaused(){
        return IsPaused;
    }
   
}
