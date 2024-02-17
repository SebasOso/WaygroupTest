using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionPickUp : MonoBehaviour
{
    [SerializeField] InventoryItem itemToPick = null;
    [SerializeField] float respawn = 5f;
    [SerializeField] GameObject pickupUI;
    [SerializeField] private bool isNear;
    private void Start()
    {
        pickupUI.SetActive(false);
    }

    private void Update()
    {
        if (itemToPick != null && Input.GetKeyDown(KeyCode.E) && isNear)
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        bool foundSlot = MenuManager.Instance.AddToFirstEmptySlot(itemToPick);
        if (foundSlot)
        {
            pickupUI.SetActive(false);
            StartCoroutine(HideForSeconds(respawn));
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isNear = true;
            pickupUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isNear = false;
            pickupUI.SetActive(false);
        }
    }

    private IEnumerator HideForSeconds(float seconds)
    {
        ToggleShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ToggleShowPickup(true);
    }

    private void ToggleShowPickup(bool toggle)
    {
        GetComponent<Collider>().enabled = toggle;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(toggle);
        }
    }
}
