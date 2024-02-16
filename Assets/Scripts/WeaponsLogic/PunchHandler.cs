using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PunchHandler : MonoBehaviour
{
    [SerializeField] private GameObject punchLogic;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ScreenShakeProfile profileCross;
    [SerializeField] private ScreenShakeProfile profileHook;
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    public void EnablePunch()
    {
        punchLogic.SetActive(true);
    }
    public void DisablePunch()
    {
        punchLogic.SetActive(false);
    }
    public void PlayRandomSoundPunch()
    {
        if (audioClips.Count > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Count);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
    }
    public void ScreenShakeCross()
    {
        CameraShakeManager.Instance.ScreenShakeFromProfileFreeLook(cinemachineImpulseSource, profileCross);
    }
    public void ScreenShakeHook()
    {
        CameraShakeManager.Instance.ScreenShakeFromProfileFreeLook(cinemachineImpulseSource, profileHook);
    }
}
