using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Combat;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private UIStateMachine uIStateMachine;
    [SerializeField] private Weapon weapon = null;
    [SerializeField] GameObject pickupUI;
    [SerializeField] InventoryItem weaponInventory;
    GameObject player;
    private InputReader InputReader;
    Weapon weaponToPick = null;
    private void Awake() 
    {
        player = GameObject.FindWithTag("Player");
        InputReader = player.GetComponent<InputReader>();
    }
    public void PickUp()
    {
        if(weaponInventory != null)
        {
            bool foundSlot = MenuManager.Instance.AddToFirstEmptySlot(weaponInventory);
            if(foundSlot)
            {
                weaponToPick = null;
                pickupUI.SetActive(false);
                StartCoroutine(HideForSeconds(5f));
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<Armory>().weaponToPickUp = this;
            InputReader.IsInteracting = true;
            weaponToPick = weapon;
            pickupUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<Armory>().weaponToPickUp = null;
            InputReader.IsInteracting = false;
            weaponToPick = null;
            pickupUI.SetActive(false);
        }
    }
    private IEnumerator HideForSeconds(float seconds)
    {
        ToggleShowPickup(false);
        yield return new WaitForSeconds(player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length);
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
