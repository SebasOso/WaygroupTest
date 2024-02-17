using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class EnemySwordDamage : MonoBehaviour
{
    private List<Collider> alreadyColliderWith = new List<Collider>();
    [SerializeField] private Collider myCollider;
    [SerializeField] private Weapon enemySword;
    [SerializeField] private List<AudioClip> audioClips = new List<AudioClip>();
    private EnemyArmory enemyArmory;
    public float damage;
    private void OnEnable() 
    {
        alreadyColliderWith.Clear();
    }
    private void OnTriggerEnter(Collider other) 
    {
        enemyArmory = myCollider.GetComponent<EnemyArmory>();
        damage = enemyArmory.damage;
        if(other.tag == "Enemy"){return;}
        if(other == myCollider){return;}
        if(alreadyColliderWith.Contains(other))
        {   
            return;
        }
        alreadyColliderWith.Add(other);
        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(20f);
            if(health.tag == "Player")
            {
                PlayRandomSound(other.GetComponent<AudioSource>());
                PlayerLife.Instance.lerpTimer = 0f;
            }
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
