using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private int damage = 11;
    public float damageDelay = 1.0f;
    private bool canDamage = true;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && canDamage)
        {
            other.GetComponent<Health>().DealDamage(damage);
            canDamage = false;
            StartCoroutine(DamageDelay());
        }
    }

    private IEnumerator DamageDelay()
    {
        yield return new WaitForSeconds(damageDelay);
        canDamage = true;
    }
}
