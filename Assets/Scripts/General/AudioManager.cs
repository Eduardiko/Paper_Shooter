using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioClip[] clipCollection = new AudioClip[8];
    [SerializeField] AudioClip[] musicClips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;
    private AudioClip currentMusicClip;


    public static AudioManager Instance { get; private set; } 

    private void Awake()
    {
        Instance = this;
        audioSource.spatialBlend = 0f;

        musicSource.loop = true;
        musicSource.spatialBlend = 0f;
        PlayMusic();
    }

    public void PlaySFX(int clipIndex, float volume = 1f)
    {
        if (clipCollection[clipIndex] != null)
            audioSource.PlayOneShot(clipCollection[clipIndex], volume);
    }

    public void PlayMusic(int clipIndex = 0, float volume = 0.75f, bool loops = true)
    {
        if (currentMusicClip == musicClips[clipIndex])
            return;

        musicSource.Stop();
        musicSource.clip = musicClips[clipIndex];
        currentMusicClip = musicClips[clipIndex];
        musicSource.volume = volume;
        musicSource.loop = loops;
        musicSource.Play();
    }
}
