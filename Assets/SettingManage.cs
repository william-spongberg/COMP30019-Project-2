using UnityEngine;
using UnityEngine.UI;

public class SettingManage : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;  // Slider for mouse sensitivity
    [SerializeField] private Slider fovSlider;          // Slider for FOV
    [SerializeField] private Button applyButton;        // Apply button

    private MovementV2 playerMovement;                  // Reference to Movement script
    private Camera playerCamera;                        // Reference to player's camera

    [SerializeField] private PauseManagerScript pauseMenu;

    void Start()
    {
        // Get references to the player's movement script and camera
        playerMovement = FindObjectOfType<MovementV2>();
        playerCamera = Camera.main;


        // Load saved settings from PlayerPrefs (if they exist)
        //float savedSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", playerMovement.mouseSensitivity);
        float savedFOV = PlayerPrefs.GetFloat("FOV", playerCamera.fieldOfView);
        
        // Initialize sliders with the current sensitivity and FOV values
        //sensitivitySlider.value = savedSensitivity;
        fovSlider.value = savedFOV;

        sensitivitySlider.onValueChanged.AddListener(UpdateMouseSensitivity);
        fovSlider.onValueChanged.AddListener(UpdateFOV);

        // Add listener to the apply button to call ApplySettings() when clicked
        applyButton.onClick.AddListener(ApplySettings);
    }

void Update()
{
    // Continuously apply the sensitivity and FOV while the sliders are being adjusted
    //playerMovement.mouseSensitivity = sensitivitySlider.value;
    playerCamera.fieldOfView = fovSlider.value;
}

    public void UpdateMouseSensitivity(float newValue)
    {
        Debug.Log("Slider moved. New sensitivity: " + newValue);
        //playerMovement.mouseSensitivity = newValue; // Use existing property
        PlayerPrefs.SetFloat("MouseSensitivity", newValue);
        Debug.Log("Mouse Sensitivity updated: " + newValue);
    }

    // Called when the FOV slider is moved
    public void UpdateFOV(float newValue)
    {
        playerCamera.fieldOfView = newValue; // Directly use the camera field of view
        PlayerPrefs.SetFloat("FOV", newValue);
        Debug.Log("FOV updated: " + newValue);
    }

    public void ApplySettings()
    {
        // Get values from sliders
        float newSensitivity = sensitivitySlider.value;
        float newFOV = fovSlider.value;

        // Apply new sensitivity and FOV values
        //playerMovement.mouseSensitivity = newSensitivity;
        playerCamera.fieldOfView = newFOV;

        Debug.Log("Settings Applied: Sensitivity = " + newSensitivity + ", FOV = " + newFOV);

        // Close settings menu
        pauseMenu.CloseSettings();
    }
}
