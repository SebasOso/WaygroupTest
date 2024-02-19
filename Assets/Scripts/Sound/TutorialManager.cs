using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    [Header("Objects to active")]
    [SerializeField] private GameObject objectsToGrab;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip record01;
    [SerializeField] private AudioClip record02;
    [SerializeField] private AudioClip record03;
    [SerializeField] private AudioClip record04;

    public event Action OnRecord01;
    public event Action OnRecord03;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Play01();   
        InteractionManager.Instance.OnFirstGrab += Play03;
        OnRecord01 += EnableObjects;
        ThrowManager.Instance.OnFirstThrow += Play04;
    }
    private void SetAudioAndPlay(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    public void Play01()
    {
        StartCoroutine(WaitForAudio01());
    }
    public void Play02()
    {
        SetAudioAndPlay(record02);
    }
    public void Play03()
    {
        StartCoroutine(WaitForAudio03());
    }
    public void Play04()
    {
        SetAudioAndPlay(record04);
    }
    private IEnumerator WaitForAudio01()
    {
        SetAudioAndPlay(record01);
        yield return new WaitForSeconds(record01.length);
        OnRecord01?.Invoke();
        Play02();
    }
    private IEnumerator WaitForAudio03()
    {
        SetAudioAndPlay(record03);
        yield return new WaitForSeconds(record03.length);
        OnRecord03?.Invoke();
    }
    private void EnableObjects()
    {
        objectsToGrab.SetActive(true);
    }
}
