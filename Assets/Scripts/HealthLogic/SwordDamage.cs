using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    private List<Collider> alreadyColliderWith = new List<Collider>();
    [SerializeField] private Collider myCollider;
    [SerializeField] private Weapon sword;
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
            health.DealDamage(sword.GetWeaponDamage());
            if (health.tag == "Player")
            {
                PlayerLife.Instance.lerpTimer = 0f;
            }
        }
    }
}
