using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Sound clips for various actions
    [SerializeField] private List<AudioClip> armingSounds;
    [SerializeField] private List<AudioClip> shellGrabSounds; 
    [SerializeField] private List<AudioClip> shellInSounds; 
    [SerializeField] private List<AudioClip> reloadCompleteSounds; 
    [SerializeField] private List<AudioClip> firingSounds;
    [SerializeField] private List<AudioClip> shellEjectSounds; 
    [SerializeField] private List<AudioClip> emptyShellSounds; 

    private int lastArmingSoundIndex = -1;
    private int lastShellGrabIndex = -1;
    private int lastShellInSoundIndex = -1;
    private int lastReloadCompleteSoundIndex = -1;
    private int lastFiringSoundIndex = -1;
    private int lastShellEjectSoundIndex = -1;
    private int lastEmptyShellSoundIndex = -1;


    // Method to play a random arming sound
    public void PlayArmingSound()
    {
        PlayRandomSound(armingSounds, ref lastArmingSoundIndex);
    }
    // Method to play a random arming sound
    private void PlayShellGrabSound()
    {
        PlayRandomSound(shellGrabSounds, ref lastShellGrabIndex);
    }
    // Method to play a random firing sound
    public void PlayRandomFiringSound()
    {
        // Play the firing sound
        PlayRandomSound(firingSounds, ref lastFiringSoundIndex);

        // Play the shell ejecting sound after a slight delay to simulate real timing
        StartCoroutine(PlayShellEjectAfterDelay());
    }

    // Coroutine to handle the shell ejection sound with a delay
    private IEnumerator PlayShellEjectAfterDelay()
    {
        // Adjust delay as desired
        yield return new WaitForSeconds(0.2f);

        PlayShellEjectSound();
    }

    // Play empty shell sound when no ammo
    public void PlayEmptyShell()
    {
        PlayRandomSound(emptyShellSounds, ref lastEmptyShellSoundIndex);
    }


    // Function to handle reloading, one shell at a time
    public void PlayReloadSequence(int totalShellsToLoad)
    {
        StartCoroutine(ReloadShellByShell(totalShellsToLoad));
    }

    private IEnumerator ReloadShellByShell(int totalShells)
    {
        
        // Play Shell Grab sound
        PlayShellGrabSound();

        // Wait for the Shell grab sound to finish before loading the next shell
        yield return new WaitForSeconds(shellGrabSounds[lastShellGrabIndex].length);

        for (int i = 0; i < totalShells-2; i++)
        {
            // Play random shell-in sound
            PlayRandomSound(shellInSounds, ref lastShellInSoundIndex);

            // Wait for the shell-in sound to finish before loading the next shell
            yield return new WaitForSeconds(shellInSounds[lastShellInSoundIndex].length + 0.3f);
        }

        // Play random reload complete sound if it exists
        if (reloadCompleteSounds.Count > 0)
        {
            PlayRandomSound(reloadCompleteSounds, ref lastReloadCompleteSoundIndex);
        }
    }

    // Play shell ejection sound after firing
    private void PlayShellEjectSound()
    {
        PlayRandomSound(shellEjectSounds, ref lastShellEjectSoundIndex);
    }


    // General method to play a random sound from a list
    private void PlayRandomSound(List<AudioClip> soundList, ref int lastSoundIndex)
    {
        if (soundList.Count > 0)
        {
            int newSoundIndex;
            do
            {
                newSoundIndex = Random.Range(0, soundList.Count);
            } while (newSoundIndex == lastSoundIndex);

            lastSoundIndex = newSoundIndex;
            audioSource.PlayOneShot(soundList[newSoundIndex]);
        }
    }
}
