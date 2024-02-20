using RPG.Inventories;
using UnityEngine;

/// <summary>
/// Represents an health pickup object in the game world, which can be interacted with by the player.
/// Inherits from the Interactable base class.
/// </summary>
public class HealthPotionPickUp : Interactable
{
    [SerializeField] InventoryItem itemToPick = null; 
    [SerializeField] GameObject pickupUI;
    private bool canPick = false;

    private void Start()
    {
        pickupUI.SetActive(false); 
    }

    /// <summary>
    /// Picks up the health potion if possible.
    /// </summary>
    private void Pickup()
    {
        if (!canPick) { return; } 
        if (itemToPick != null)
        {
            MenuManager.Instance.PlayEquip(); 
            bool foundSlot = MenuManager.Instance.AddToFirstEmptySlot(itemToPick); 
            if (foundSlot)
            {
                pickupUI.SetActive(false); 
                gameObject.SetActive(false); 
            }
            else
            {
                pickupUI.SetActive(false); 
                gameObject.SetActive(false); 
            }
        }
    }

    /// <summary>
    /// Handles the interaction event when the player interacts with the health pickup.
    /// </summary>
    public override void OnInteract()
    {
        Pickup(); 
        TutorialManager.Instance.CheckPickUps(); 
    }

    /// <summary>
    /// Handles the focus event when the player interacts with the health pickup.
    /// </summary>
    public override void OnFocus()
    {
        pickupUI.SetActive(true); 
        canPick = true; 
        GetComponent<Outline>().enabled = true;
    }

    /// <summary>
    /// Handles the lose focus event when the player interacts with the health pickup.
    /// </summary>
    public override void OnLoseFocus()
    {
        pickupUI.SetActive(false); 
        canPick = false; 
        GetComponent<Outline>().enabled = false; 
    }
}
