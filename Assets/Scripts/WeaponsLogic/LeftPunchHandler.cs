using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPunchHandler : MonoBehaviour
{
    [SerializeField] private GameObject leftPunchLogic;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;
    public void EnableLeftPunch()
    {
        leftPunchLogic.SetActive(true);
    }
    public void DisableLeftPunch()
    {
        leftPunchLogic.SetActive(false);
    }
    public void PlayRandomSoundLeftPunch()
    {
        if (audioClips.Count > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Count);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
    }
}
