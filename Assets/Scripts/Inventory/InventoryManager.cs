using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using Newtonsoft.Json.Linq;
using RPG.Combat;
using RPG.Saving;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] itemsInInventory;

    [Header("Weapons")]
    public InventoryItem weaponEquipped;
    public InventoryItem weaponInBack;

    [Header("Armor")]
    public InventoryItem shoulderEquipped;

    [Header("Sounds Settings")]
    [SerializeField] private AudioClip equipClip;
    [SerializeField] private AudioSource audioSource;
    public static InventoryManager Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start() 
    {
        Redraw();
        if(weaponEquipped == null)
        {
            weaponEquipped = Armory.Instance.currentWeapon.value.GetInventoryItem();
        }
    }
    private void OnEnable() 
    {
        Redraw();
        weaponEquipped = Armory.Instance.currentWeapon.value.GetInventoryItem();
        if(ShoulderArmorManager.Instance.shoulder != null)
        {
            shoulderEquipped = ShoulderArmorManager.Instance.shoulder.GetInventoryItem();
        }
        if(Armory.Instance.disarmedWeapon != null)
        {
            weaponInBack = Armory.Instance.disarmedWeapon.GetInventoryItem();
        }
        else
        {
            weaponInBack = null;
        }
    }
    private void Redraw()
    {
        for (int i = 0; i < MenuManager.Instance.GetSize(); i++)
        {
            if(MenuManager.Instance.GetItemInSlot(i) == null)
            {
                continue;
            }
            itemsInInventory[i].SetItem(i);
        }
    }
    private IEnumerator SetNewWeapon(InventoryItem weaponToEquip, Item itemToDeleteFromInventory)
    {
        yield return new WaitForSeconds(0.1f);
        if(weaponEquipped != null)
        {
            MenuManager.Instance.AddToFirstEmptySlot(weaponEquipped);
        }
        else if(weaponInBack != null)
        {
            MenuManager.Instance.AddToFirstEmptySlot(weaponInBack);
        }
        itemToDeleteFromInventory.DeleteItemFromInventory();
        PlayEquipSound();
        weaponEquipped = weaponToEquip;
        Redraw();
    }
    private IEnumerator SetNewShoulder(InventoryItem shoulderToEquip, Item itemToDeleteFromInventory)
    {
        yield return new WaitForSeconds(0.1f);
        if (shoulderEquipped != null)
        {
            MenuManager.Instance.AddToFirstEmptySlot(shoulderEquipped);
        }
        itemToDeleteFromInventory.DeleteItemFromInventory();
        PlayEquipSound();
        shoulderEquipped = shoulderToEquip;
        Redraw();
    }
    private void PlayEquipSound()
    {
        audioSource.clip = equipClip;
        audioSource.Play();
    }

    //Setters
    public void SetNewInventoryWeapon(InventoryItem weaponToEquip, Item itemToDeleteFromInventory)
    {
        StartCoroutine(SetNewWeapon(weaponToEquip, itemToDeleteFromInventory));
    }
    public void SetNewInventoryShoulder(InventoryItem shoulderToEquip, Item itemToDeleteFromInventory)
    {
        StartCoroutine(SetNewShoulder(shoulderToEquip, itemToDeleteFromInventory));
    }
}
