using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private List<Collider> alreadyColliderWith = new List<Collider>();
    Health target = null;
    private Collider targetCollider;
    [SerializeField] bool isHoming = true;
    [SerializeField] float speed = 1;
    [SerializeField] float maxLifeTime = 3f;
    float damage = 0;
    private bool isFreezeTime = false;
    private void Start() 
    {
        if(target == null)
        {
            transform.LookAt(Vector3.forward);
        }
        transform.LookAt(GetAimLocation());
    }
    void Update()
    {
        if(target == null){return;}
        if(isHoming && !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
        Destroy(gameObject, maxLifeTime);
    }
    private Vector3 GetAimLocation() 
    {
        if (!targetCollider) {
            targetCollider = target.GetComponent<Collider>();
        }
    
        if (!targetCollider) {
            return target.transform.position;
        }
        return targetCollider.bounds.center;
    }
    private void OnEnable() 
    {
        alreadyColliderWith.Clear();
    }
    public void SetFreeze()
    {
        isFreezeTime = true;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.GetComponent<Health>() != target){return;}
        if(alreadyColliderWith.Contains(other))
        {   
            return;
        }
        alreadyColliderWith.Add(other);
        if(target.IsDead()){return;}
        speed = 0f;
        if(other.TryGetComponent<Health>(out Health health))
        {
            health.DealArrowDamage(damage);
            health.PlayArrowImpact();
            if (health.tag == "Enemy")
            {
                if (!health.GetComponent<EnemyStateMachine>().isStunned)
                {
                    health.GetComponent<EnemyStateMachine>().EnemyAggro();
                }
                if (isFreezeTime)
                {
                    health.GetComponent<EnemyStateMachine>().EnemyStun();
                    Destroy(gameObject);
                }
            }
            if (health.tag == "Player")
            {
                PlayerLife.Instance.lerpTimer = 0f;
            }
        }
        Destroy(gameObject);
    }
}
