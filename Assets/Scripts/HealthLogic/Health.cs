using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;


/// <summary>
/// Manages the health and damage-taking functionality of a character (enemies and player).
/// </summary>
public class Health : MonoBehaviour
{
    [Header("Armor")]
    public float armor;

    //Events
    public event Action OnDie;
    public event Action OnTakeDamage;
    public event Action OnHeal;

    //Health Main Variable
    public float health = 200;

    //Booleans
    private bool isDead = false;
    private bool isInvulnerable;
    [Header("Bool For Debug")]
    public bool isFullHealth = true;

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
    private void Update()
    {
        if(health < 200)
        {
            isFullHealth = false;
        }
        else if (health >= 200)
        {
            isFullHealth = true;
        }
    }

    /// <summary>
    /// Deals damage to the character.
    /// </summary>
    /// <param name="damage">The amount of damage to deal.</param>
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
        Debug.Log("Loss Health = " + finalDamage);
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

    /// <summary>
    /// Heals the character over time.
    /// </summary>
    /// <param name="regenerationRate">The rate of health regeneration.</param>
    public void HealthPotion(float regenerationRate)
    {
        if(TutorialManager.Instance.isFirstHeal == false)
        {
            TutorialManager.Instance.isFirstHeal = true;
            OnHeal?.Invoke();
        }
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

    /// <summary>
    /// Deals arrow damage to the character.
    /// </summary>
    /// <param name="damage">The amount of arrow damage.</param>
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

    /// <summary>
    /// Plays the arrow impact sound.
    /// </summary>
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
