using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using RPG.Combat;
using UnityEngine;

public class AxeDamage : MonoBehaviour
{
    private List<Collider> alreadyColliderWith = new List<Collider>();
    [SerializeField] private Collider myCollider;
    private Armory armory;
    public bool runeDamage = false;
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
        armory = myCollider.GetComponent<Armory>();
        if(other == myCollider){return;}
        if(alreadyColliderWith.Contains(other))
        {   
            return;
        }
        alreadyColliderWith.Add(other);
        if(other.TryGetComponent<Health>(out Health health))
        {
            if(runeDamage)
            {
                PlayRuneSound();
                CameraShakeManager.Instance.ScreenShakeFromProfile(cinemachineImpulseSource, profile);
                damage = armory.damage + 10f;
                health.DealDamage(damage);
                HealManager.Instance.AddHitHeal();
            }
            else
            {
                PlayRandomSound();
                CameraShakeManager.Instance.ScreenShakeFromProfile(cinemachineImpulseSource, profile);
                damage = armory.damage;
                health.DealDamage(damage);
                HealManager.Instance.AddHitHeal();
            }
            if(health.tag == "Player")
            {
                PlayRandomSound();
                PlayerLife.Instance.lerpTimer = 0f;
            }
        }
        if(other.TryGetComponent<ForceReceiver>(out ForceReceiver force))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            force.AddForce(direction * armory.currentWeapon.value.GetWeaponKnokcback());
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
    private void PlayRuneSound()
    {
        audioSource.clip = audioClipRune;
        audioSource.Play();
    }
}
