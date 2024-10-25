using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;


    [SerializeField] private List<AudioClip> hitSounds;  

    private int lastHitSoundIndex = -1;

    // Method to play a random hit sound (called when the weapon hits something)
    public void PlayObjectHitSound()
    {
        PlayRandomSound(hitSounds, ref lastHitSoundIndex);
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
