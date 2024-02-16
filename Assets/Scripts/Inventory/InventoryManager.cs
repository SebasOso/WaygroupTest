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
    }
    private void OnEnable() 
    {
        Redraw();
        if(ShoulderArmorManager.Instance.shoulder != null)
        {
            shoulderEquipped = ShoulderArmorManager.Instance.shoulder.GetInventoryItem();
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
    public void SetNewInventoryShoulder(InventoryItem shoulderToEquip, Item itemToDeleteFromInventory)
    {
        StartCoroutine(SetNewShoulder(shoulderToEquip, itemToDeleteFromInventory));
    }
}
