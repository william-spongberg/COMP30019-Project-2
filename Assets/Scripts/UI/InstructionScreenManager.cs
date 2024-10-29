using UnityEngine;

public class InstructionsScreenManager : MonoBehaviour
{
    [SerializeField] private PauseManagerScript pauseManagerScript;

    void Start()
    {
        Invoke(nameof(PauseGame), 0.5f);
        Invoke(nameof(OpenInstructions), 0.5f);
    }

    void PauseGame()
    {
        pauseManagerScript.Pause();
    }

    void OpenInstructions()
    {
        pauseManagerScript.OpenInstructionsPageOne();
    }
}
