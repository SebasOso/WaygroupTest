using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicHandler : MonoBehaviour
{
    public AudioClip[] combatAudioClips;
    public AudioClip[] musicAudioClips;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomMusicAudioClip();
    }
    public void PlayRandomMusicAudioClip()
    {
        int randomIndex = Random.Range(0, musicAudioClips.Length);

        audioSource.clip = musicAudioClips[randomIndex];
        audioSource.Play();
    }
}
