using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.Combat;
using UnityEngine;

public class PunchDamage : MonoBehaviour
{
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    private List<Collider> alreadyColliderWith = new List<Collider>();
    [SerializeField] private Collider myCollider;
    [SerializeField] private float damage;
    [SerializeField] private CinemachineImpulseSource cinemachineImpulseSource;
    [SerializeField] private ScreenShakeProfile profile;
    [SerializeField] private AudioSource audioSource;
    private void OnEnable() 
    {
        alreadyColliderWith.Clear();
    }
    private void OnTriggerEnter(Collider other) 
    {
        damage = 20;
        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(damage);
            if (health.tag == "Player")
            {
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
