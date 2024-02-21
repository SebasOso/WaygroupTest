using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class EnemyAxeDamage : MonoBehaviour
{
    private List<Collider> alreadyColliderWith = new List<Collider>();
    [SerializeField] private Collider myCollider;
    private EnemyArmory enemyArmory;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    public float damage;
    private void OnEnable() 
    {
        alreadyColliderWith.Clear();
    }
    private void OnTriggerEnter(Collider other) 
    {
        enemyArmory = myCollider.GetComponent<EnemyArmory>();
        if(other.tag == "Enemy"){return;}
        if(other == myCollider){return;}
        if(alreadyColliderWith.Contains(other))
        {   
            return;
        }
        alreadyColliderWith.Add(other);
        if(other.TryGetComponent<Health>(out Health health))
        {
            damage = enemyArmory.damage;
            health.DealDamage(damage);
            if(health.tag == "Player")
            {
                PlayRandomSound(other.GetComponent<AudioSource>());
                PlayerLife.Instance.lerpTimer = 0f;
            }
        }
        if(other.TryGetComponent<ForceReceiver>(out ForceReceiver force))
        {
            Vector3 direction = (other.transform.position - myCollider.transform.position).normalized;
            force.AddForce(direction * enemyArmory.currentWeapon.GetWeaponKnokcback());
        }
    }
    private void PlayRandomSound(AudioSource audioSource)
    {
        if (audioClips.Count > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Count);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
    }
}
