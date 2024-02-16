using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundManager : MonoBehaviour
{
    [SerializeField] AudioClip hitClip;
    [SerializeField] AudioClip dieClip;
    public void PlayHit()
    {
        GetComponent<AudioSource>().clip = hitClip;
        GetComponent<AudioSource>().Play();
    }
    public void PlayDie()
    {
        GetComponent<AudioSource>().clip = dieClip;
        GetComponent<AudioSource>().Play();
    }
}
