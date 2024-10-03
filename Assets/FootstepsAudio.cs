using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsAudio : MonoBehaviour
{
    // Reference to the Footsteps GameObject
    public GameObject Footsteps;
    private AudioSource footstepAudioSource;

    // Variables to track the player's movement input
    float horizontalInput;
    float verticalInput;

    // Start is called before the first frame update
    void Start()
    {
        // Initially disable the footsteps audio
        footstepAudioSource = Footsteps.GetComponent<AudioSource>();
        Footsteps.SetActive(false);  // Make sure footsteps are off initially
    }

    // Update is called once per frame
    void Update()
    {
        // Get the horizontal and vertical input axes (for WASD and arrow keys)
        horizontalInput = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrow
        verticalInput = Input.GetAxisRaw("Vertical");     // W/S or Up/Down Arrow

        // If there is any input (WASD or arrow keys), trigger footsteps
        if (horizontalInput != 0 || verticalInput != 0)
        {
            setFootstepsAudio();
        }

        // If no input is detected, stop the footsteps
        if (horizontalInput == 0 && verticalInput == 0)
        {
            StopFootstepsAudio();
        }
    }

    // Function to start the footsteps audio
    void setFootstepsAudio()
    {
        if (!Footsteps.activeSelf) 
            Footsteps.SetActive(true); // Activate the GameObject

        if (!footstepAudioSource.isPlaying)  // Check if it's not already playing
            footstepAudioSource.Play();     // Play the audio
    }

    // Function to stop the footsteps audio
    void StopFootstepsAudio()
    {
        if (footstepAudioSource.isPlaying)  // Stop the audio if it's playing
            footstepAudioSource.Stop();

        Footsteps.SetActive(false);  // Deactivate the GameObject
    }
}
