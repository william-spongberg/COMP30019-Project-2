using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Sound clips for various actions
    [SerializeField] private AudioClip armingSound;
    [SerializeField] private AudioClip clipInSound;
    [SerializeField] private AudioClip clipOutSound;
    [SerializeField] private AudioClip reloadSequenceSound;

    // Multiple firing sounds (e.g. SFX)
    [SerializeField] private List<AudioClip> firingSounds;

    private int lastFiringSoundIndex = -1;

    public void PlayArmingSound()
    {
        audioSource.PlayOneShot(armingSound);
    }

    public void PlayReloadSequenceSound()
    {
        audioSource.PlayOneShot(reloadSequenceSound);
    }

    public void PlayRandomFiringSound()
    {
        // Avoid playing the same sound twice in a row
        if (firingSounds.Count > 0)
        {
            int newFiringSoundIndex;
            do
            {
                newFiringSoundIndex = Random.Range(0, firingSounds.Count);
            } while (newFiringSoundIndex == lastFiringSoundIndex);

            lastFiringSoundIndex = newFiringSoundIndex;
            audioSource.PlayOneShot(firingSounds[newFiringSoundIndex]);
        }
    }
    // Function to start the sound sequence
    public void PlayClipOutIn()
    {
        StartCoroutine(ReloadSoundCoroutine());
    }

    private IEnumerator ReloadSoundCoroutine()
    {
        // Play clip out sound first
        audioSource.PlayOneShot(clipOutSound);

        // Wait for the clip out sound to finish
        yield return new WaitForSeconds(clipOutSound.length);

        // Play clip in sound after clip out
        audioSource.PlayOneShot(clipInSound);

        // Wait for the clip in sound to finish
        yield return new WaitForSeconds(clipInSound.length);
    }
}
