using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    // Sound clips for melee weapon actions
    [SerializeField] private AudioClip armingSound;
    [SerializeField] private List<AudioClip> hitSounds;  // Sounds for hitting something
    [SerializeField] private List<AudioClip> missSounds; // Sounds for missing the target

    private int lastHitSoundIndex = -1;
    private int lastMissSoundIndex = -1;

    // Method to play arming sound when weapon is equipped
    public void PlayArmingSound()
    {
        audioSource.PlayOneShot(armingSound);
    }


    // Method to play a random hit sound (called when the weapon hits something)
    public void PlayHitSound()
    {
        PlayRandomSound(hitSounds, ref lastHitSoundIndex);
    }

    // Method to play a random miss sound (called when the weapon misses)
    public void PlayMissSound()
    {
        PlayRandomSound(missSounds, ref lastMissSoundIndex);
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
