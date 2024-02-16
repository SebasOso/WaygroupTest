using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.Combat;
using UnityEngine;

public class AxeDamage : MonoBehaviour
{
    private List<Collider> alreadyColliderWith = new List<Collider>();
    [SerializeField] private Collider myCollider;
    public float damage;
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] private ScreenShakeProfile profile;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    [SerializeField] private AudioClip audioClipRune;
    [SerializeField] private AudioSource audioSource;
    private void OnEnable() 
    {
        alreadyColliderWith.Clear();
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other == myCollider){return;}
        if(alreadyColliderWith.Contains(other))
        {   
            return;
        }
        alreadyColliderWith.Add(other);
        if(other.TryGetComponent<Health>(out Health health))
        {
            PlayRandomSound();
            damage = 20;
            health.DealDamage(damage);
            if(health.tag == "Player")
            {
                PlayRandomSound();
                PlayerLife.Instance.lerpTimer = 0f;
            }
        }
    }
    private void PlayRandomSound()
    {
        if (audioClips.Count > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Count);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
    }
}
