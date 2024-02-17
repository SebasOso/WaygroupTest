using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPickUp : Interactable
{
    [SerializeField] InventoryItem itemToPick = null;
    [SerializeField] GameObject pickupUI;
    private bool canPick = false;

    private void Start()
    {
        pickupUI.SetActive(false);
    }
    private void Pickup()
    {
        if (!canPick) { return; }
        if (itemToPick != null)
        {
            bool foundSlot = MenuManager.Instance.AddToFirstEmptySlot(itemToPick);
            if (foundSlot)
            {
                pickupUI.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canPick = true;
            pickupUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canPick = false;
            pickupUI.SetActive(false);
        }
    }

    public override void OnInteract()
    {
        Pickup();
    }

    public override void OnFocus()
    {
        pickupUI.SetActive(true);
        canPick = true;
        GetComponent<Outline>().enabled = true;
    }

    public override void OnLoseFocus()
    {
        pickupUI.SetActive(false);
        canPick = false;
        GetComponent<Outline>().enabled = false;
    }
}