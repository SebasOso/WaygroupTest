using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potions", menuName = "Health/Make New Health Potion", order = 0)]
public class HealthPotion : ScriptableObject
{
    [SerializeField] private float regenerationRate;
    [SerializeField] private Health playerHealth;
    public void Heal()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        playerHealth.HealthPotion(regenerationRate);
    }
}
