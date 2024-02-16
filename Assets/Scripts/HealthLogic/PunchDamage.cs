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
    private Armory armory;
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
        armory = myCollider.GetComponent<Armory>();
        damage = armory.damage;
        if(other == myCollider){return;}
        if(alreadyColliderWith.Contains(other))
        {   
            return;
        }
        alreadyColliderWith.Add(other);
        if(other.TryGetComponent<Health>(out Health health))
        {
            CameraShakeManager.Instance.ScreenShakeFromProfile(cinemachineImpulseSource, profile);
            PlayRandomSound();
            health.DealDamage(damage);
            HealManager.Instance.AddHitHeal();
            if (health.tag == "Player")
            {
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
}
