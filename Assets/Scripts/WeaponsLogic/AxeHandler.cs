using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AxeHandler : MonoBehaviour
{
    [SerializeField] private GameObject axeLogic;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip runeClip;
    [SerializeField] private ScreenShakeProfile profileDown;
    [SerializeField] private ScreenShakeProfile profileUp;
    [SerializeField] private ScreenShakeProfile profileCut;
    [SerializeField] private ScreenShakeProfile profileRune;
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    public void EnableAxe()
    {
        axeLogic.SetActive(true);
    }
    public void DisableAxe()
    {
        axeLogic.SetActive(false);
    }
    public void RuneAttack()
    {
        axeLogic.GetComponent<AxeDamage>().runeDamage = true;
        audioSource.clip = runeClip;
        audioSource.Play();
    }
    public void NoRuneAttack()
    {
        axeLogic.GetComponent<AxeDamage>().runeDamage = false;
    }
    public void PlayRandomSoundAxe()
    {
        if (audioClips.Count > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Count);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
    }
    public void ScreenShakeUp()
    {
        CameraShakeManager.Instance.ScreenShakeFromProfileFreeLook(cinemachineImpulseSource, profileUp);
    }
    public void ScreenShakeDown()
    {
        CameraShakeManager.Instance.ScreenShakeFromProfileFreeLook(cinemachineImpulseSource, profileDown);
    }
    public void ScreenShakeCut()
    {
        CameraShakeManager.Instance.ScreenShakeFromProfileFreeLook(cinemachineImpulseSource, profileCut);
    }
    public void ScreenShakeRune()
    {
        CameraShakeManager.Instance.ScreenShakeFromProfileFreeLook(cinemachineImpulseSource, profileRune);
    }
}
