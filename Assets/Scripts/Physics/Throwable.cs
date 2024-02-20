
using UnityEngine;

/// <summary>
/// Represents a throwable object in the game world, which can be interacted with by the player.
/// Inherits from the Interactable base class.
/// </summary>
public class Throwable : Interactable
{
    private bool canGrab = false;   
    private bool isDanger = false;      
    /// <summary>
    /// Updates the object's velocity each frame and handles focus loss when being carried.
    /// </summary>
    private void Update()
    {
        if (ThrowManager.Instance.isBeingCarried)
        {
            OnLoseFocus();                                       
        }
    }

    /// <summary>
    /// Handles collision events to apply force and deal damage if the object is marked as dangerous.
    /// </summary>
    private void OnCollisionEnter(Collision collision)
    {
        if (isDanger)
        {
            if (collision.collider.CompareTag("Enemy") && (GetSpeedInAir() >= 4 || GetComponent<Rigidbody>().mass >= 4))
            {
                Vector3 hitPoint = collision.contacts[0].point;

                Vector3 forceDirection = GetComponent<Rigidbody>().velocity;
                forceDirection.y = 0;
                forceDirection.Normalize();

                Vector3 force = (GetComponent<Rigidbody>().mass * GetSpeedInAir() * 10f) * forceDirection;

                // Applies force to the enemy and deals damage upon collision
                collision.collider.GetComponent<Ragdoll>().ApplyForce(force, hitPoint);
                collision.collider.GetComponent<Health>().DealDamage(300);
            }
        }
    }

    /// <summary>
    /// Handles the grab action when the player interacts with the throwable object.
    /// </summary>
    private void Grab()
    {
        if (!canGrab) { return; }
        ThrowManager.Instance.Grab(this);  
        OnLoseFocus();                    
    }

    /// <summary>
    /// Handles the interaction event when the player interacts with the throwable object.
    /// </summary>
    public override void OnInteract()
    {
        Grab();
    }

    /// <summary>
    /// Handles the event when the player focuses on the throwable object.
    /// </summary>
    public override void OnFocus()
    {
        canGrab = true; 
        GetComponent<Outline>().enabled = true; 
    }

    /// <summary>
    /// Handles the event when the player loses focus on the throwable object.
    /// </summary>
    public override void OnLoseFocus()
    {
        canGrab = false; 
        GetComponent<Outline>().enabled = false;
    }

    /// <summary>
    /// Marks the throwable object as dangerous, capable of causing damage upon collision.
    /// </summary>
    public void SetDanger()
    {
        isDanger = true; 
    }
    /// <summary>
    /// Get the max velocity of the object.
    /// </summary>
    private float GetSpeedInAir()
    {
        Vector3 currentVelocity = GetComponent<Rigidbody>().velocity;
        float currentSpeed = currentVelocity.magnitude;
        return currentSpeed;
    }
}
