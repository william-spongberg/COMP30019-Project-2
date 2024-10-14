using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepsAudio : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudioSource;

    // Footstep sounds for walking and running
    [SerializeField] private List<AudioClip> walkingFootsteps;
    [SerializeField] private List<AudioClip> runningFootsteps;

    private float stepCooldown;

    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float sprintStepInterval = 0.35f;

    private int lastFootstepIndex = -1;

    void Start()
    {
        stepCooldown = 0;
    }

    // Sets the appropriate footstep sound and plays it. Based on movement type (Sprint/Walk)
    public void SetFootstepsAudio(bool isSprinting)
    {
        // If the cooldown has elapsed, play a footstep sound
        if (stepCooldown <= 0f)
        {
            PlayFootstepSound(isSprinting);  // Play appropriate footstep sound
            ResetCooldown(isSprinting);      // Reset cooldown after playing
        }
    }

    public void UpdateStepCooldown(float deltaTime)
    {
        stepCooldown -= deltaTime; // Decrement the cooldown timer
    }

    // Stops playing footsteps
    public void StopFootstepsAudio()
    {
        if (footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
        }
    }

    // Play a random footstep sound, depending on whether the player is sprinting or walking
    private void PlayFootstepSound(bool isSprinting)
    {
        List<AudioClip> chosenFootstepSounds;

        // Choose the right footstep sounds based on sprinting or walking
        if (isSprinting)
        {
            chosenFootstepSounds = runningFootsteps;
        }
        else
        {
            chosenFootstepSounds = walkingFootsteps;
        }

        // Play the footstep sound
        if (chosenFootstepSounds.Count > 0)
        {
            int newFootstepIndex;

            // Ensure we don't play the same sound twice in a row
            do
            {
                newFootstepIndex = Random.Range(0, chosenFootstepSounds.Count);
            } while (newFootstepIndex == lastFootstepIndex);

            lastFootstepIndex = newFootstepIndex;

            // Assign and play the selected footstep sound
            footstepAudioSource.clip = chosenFootstepSounds[newFootstepIndex];
            footstepAudioSource.Play();
        }
    }

    // Reset cooldown based on the movement type
    private void ResetCooldown(bool isSprinting)
    {
        if (isSprinting)
        {
            stepCooldown = sprintStepInterval;
        }
        else
        {
            stepCooldown = walkStepInterval;
        }
    }
}
