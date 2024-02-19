using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Interactable
{
    private bool canGrab = false;
    private bool isDanger = false;
    public float velocity = 0;
    public float debugForce = 20;
    private void Update()
    {
        if(ThrowManager.Instance.isBeingCarried)
        {
            OnLoseFocus();
        }
        velocity = GetComponent<Rigidbody>().velocity.magnitude;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(isDanger)
        {
            if(collision.collider.CompareTag("Enemy") && (velocity >= 4 || GetComponent<Rigidbody>().mass >= 4))
            {
                Vector3 hitPoint = collision.contacts[0].point;

                Vector3 forceDirection = GetComponent<Rigidbody>().velocity;
                forceDirection.y = 0;
                forceDirection.Normalize();

                Vector3 force = (GetComponent<Rigidbody>().mass * velocity * debugForce) * forceDirection;

                collision.collider.GetComponent<Ragdoll>().ApplyForce(force, hitPoint);
                collision.collider.GetComponent<Health>().DealDamage(300);
            }
        }
    }
    private void Grab()
    {
        if (!canGrab) { return; }
        ThrowManager.Instance.Grab(this);
        OnLoseFocus();
    }
    public override void OnInteract()
    {
        Grab();
    }

    public override void OnFocus()
    {
        canGrab = true;
        GetComponent<Outline>().enabled = true;
    }

    public override void OnLoseFocus()
    {
        canGrab = false;
        GetComponent<Outline>().enabled = false;
    }
    public void SetDanger()
    {
        isDanger = true;
    }
}
