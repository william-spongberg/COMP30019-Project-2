using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFootstepsAudio : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudioSource;

    // Footstep sounds for walking and running
    [SerializeField] private List<AudioClip> walkingFootsteps;
    [SerializeField] private List<AudioClip> runningFootsteps;

    private int lastFootstepIndex = -1;

    // Stops playing footsteps
    public void StopFootstepsAudio()
    {
        if (footstepAudioSource.isPlaying)
        {
            footstepAudioSource.Stop();
        }
    }

    // Play a random footstep sound, depending on whether the player is sprinting or walking
    public void PlayFootstepSound(bool isSprinting)
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

            Debug.Log("Playing sound: " + footstepAudioSource.clip.name);
            
            // Assign and play the selected footstep sound
            footstepAudioSource.PlayOneShot(chosenFootstepSounds[newFootstepIndex]);
            Debug.Log("Footstep sound should be playing");
        }
    }

}
