using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SettingsManager : MonoBehaviour
{
    // Sliders for sensitivity and FOV
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider fovSlider;

    // Reference to Cinemachine Virtual Camera
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;

    // Default values for sensitivity and FOV
    [SerializeField] private float defaultSensitivity = 300f;
    [SerializeField] private float defaultFOV = 50f;

    private void Start()
    {
        // Set sliders to default values
        sensitivitySlider.value = defaultSensitivity;
        fovSlider.value = defaultFOV;

        // Apply initial default values to the camera
        UpdateSensitivity(defaultSensitivity);
        UpdateFOV(defaultFOV);

        // Add listeners for slider changes to immediately apply changes
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
        fovSlider.onValueChanged.AddListener(UpdateFOV);
    }

    private void UpdateSensitivity(float newSensitivity)
    {
        // Adjust the mouse sensitivity in Cinemachine POV
        if (cinemachineCamera != null)
        {
            CinemachinePOV pov = cinemachineCamera.GetCinemachineComponent<CinemachinePOV>();
            if (pov != null)
            {
                pov.m_HorizontalAxis.m_MaxSpeed = newSensitivity;
                pov.m_VerticalAxis.m_MaxSpeed = newSensitivity;
            }
        }
    }

    private void UpdateFOV(float newFOV)
    {
        if (cinemachineCamera != null)
        {
            var lens = cinemachineCamera.m_Lens;
            lens.FieldOfView = newFOV; 
            cinemachineCamera.m_Lens = lens;
        }
    }
}
