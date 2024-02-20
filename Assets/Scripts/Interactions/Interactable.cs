using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class defining the behavior of an interactable object in the game.
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    /// <summary>
    /// Initializes the GameObject's layer to the interactable layer.
    /// </summary>
    public virtual void Awake()
    {
        this.gameObject.layer = 16; // Sets the GameObject's layer to the interactable layer
    }

    /// <summary>
    /// Called when the player interacts with the object.
    /// </summary>
    public abstract void OnInteract();

    /// <summary>
    /// Called when the object gains focus (e.g., player saw an object).
    /// </summary>
    public abstract void OnFocus();

    /// <summary>
    /// Called when the object loses focus (e.g., player lose sight of the object).
    /// </summary>
    public abstract void OnLoseFocus();
}
