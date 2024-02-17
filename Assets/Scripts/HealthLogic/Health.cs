using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Health : MonoBehaviour
{
    [Header("Armor")]
    public float armor;

    //Events
    public event Action OnDie;
    public event Action OnTakeDamage;

    //Health Main Variable
    public float health = 200;

    //Booleans
    private bool isDead = false;
    private bool isInvulnerable;
    [Header("Bool For Debug")]
    public bool isFullHealth;

    [Header("Visual Effects")]
    //Effects
    [SerializeField] private VisualEffect hit;
    [SerializeField] private ParticleSystem healingEffect;

    [Header("Sounds")]
    [SerializeField] private AudioClip arrowImpactSound;
    [SerializeField] private AudioSource impactAudioSource;

    [HideInInspector] public bool isHealing;
    private void Start()
    {
        health = 200;
    }
    public void DealDamage(float damage)
    {
        if(health <= 0){return;}
        if(isInvulnerable){return;}
        hit.Play();
        if(GetComponent<EnemyLife>())
        {
            GetComponent<EnemyLife>().lerpTimer = 0f;
        }
        float reducedDamage = armor / (armor + 100);
        float finalDamage = damage * (1 - reducedDamage);
        health = Mathf.Max(health - finalDamage, 0);
        if(gameObject.CompareTag("Enemy"))
        {
            if(GetComponent<EnemyStateMachine>().isStunned)
            {
                if (health == 0)
                {
                    OnDie?.Invoke();
                    Die();
                }
                return;
            }
            else if(!GetComponent<EnemyStateMachine>().isStunned)
            {
                OnTakeDamage?.Invoke();
            }
        }
        OnTakeDamage?.Invoke();
        if(health == 0)
        {
            OnDie?.Invoke();
            Die();
        }
    }
    public void HealthPotion(float regenerationRate)
    {
        StartCoroutine(Regeneration(regenerationRate));
    }
    private IEnumerator Regeneration(float regenerationRate)
    {
        if (health < 200f)
        {
            healingEffect.Stop();
            isHealing = false;
            var main = healingEffect.main;
            main.duration = 6f;
            healingEffect.Play();
            float healingTime = 0f;
            while (healingTime < 6f)
            {
                isHealing = true;
                float healthToAdd = regenerationRate * Time.deltaTime;
                health += healthToAdd;
                healingTime += Time.deltaTime;
                if (health >= 200f)
                {
                    health = 200f;
                    isFullHealth = true;
                    isHealing = false;
                    healingEffect.Stop();
                }
                yield return null;
            }
        }
    }
    public void DealArrowDamage(float damage)
    {
        if (health <= 0) { return; }
        if (isInvulnerable) { return; }
        hit.Play();
        if (GetComponent<EnemyLife>())
        {
            GetComponent<EnemyLife>().lerpTimer = 0f;
        }
        float reducedDamage = armor / (armor + 100);
        float finalDamage = damage * (1 - reducedDamage);
        health = Mathf.Max(health - finalDamage, 0);
        if (health == 0)
        {
            OnDie?.Invoke();
            Die();
        }
    }
    public void PlayArrowImpact()
    {
        impactAudioSource.clip = arrowImpactSound;
        impactAudioSource.Play();
    }
    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }
    private void Die()
    {
        if(isDead) return;
        isDead = true;
    }
    public bool IsDead()
    {
        return isDead;
    }
}
