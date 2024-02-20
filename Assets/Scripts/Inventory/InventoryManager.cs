using System.Collections;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the player's inventory and item interactions in the inventory UI.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] itemsInInventory; 

    private Item itemToUse; 

    [Header("Armor")]
    public InventoryItem shoulderEquipped; 

    [Header("Sound Settings")]
    [SerializeField] private AudioClip equipClip; 
    [SerializeField] private AudioSource audioSource; 

    [Header("UI Settings")]
    [SerializeField] private Button useButton; 
    [SerializeField] private Button dropButton; 

    public static InventoryManager Instance { get; set; } 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Uses the currently selected item.
    /// </summary>
    public void UseItem()
    {
        if (itemToUse != null)
        {
            itemToUse.UseObject();
        }
    }

    /// <summary>
    /// Drops the currently selected item.
    /// </summary>
    public void DropItem()
    {
        if (itemToUse != null)
        {
            itemToUse.DropObject();
        }
    }

    private void Start()
    {
        Redraw();
    }

    private void OnEnable()
    {
        Redraw();
        itemToUse = null;
        useButton.interactable = false;
        dropButton.interactable = false;
        if (ShoulderArmorManager.Instance.shoulder != null)
        {
            shoulderEquipped = ShoulderArmorManager.Instance.shoulder.GetInventoryItem();
        }
    }

    /// <summary>
    /// Redraws the inventory UI.
    /// </summary>
    private void Redraw()
    {
        for (int i = 0; i < MenuManager.Instance.GetSize(); i++)
        {
            if (MenuManager.Instance.GetItemInSlot(i) == null)
            {
                continue;
            }
            itemsInInventory[i].SetItem(i);
        }
    }

    /// <summary>
    /// Sets a new shoulder armor in the inventory.
    /// </summary>
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

    /// <summary>
    /// Plays the equip sound.
    /// </summary>
    private void PlayEquipSound()
    {
        audioSource.clip = equipClip;
        audioSource.Play();
    }

    // Setters

    /// <summary>
    /// Sets a new shoulder armor in the inventory.
    /// </summary>
    public void SetNewInventoryShoulder(InventoryItem shoulderToEquip, Item itemToDeleteFromInventory)
    {
        StartCoroutine(SetNewShoulder(shoulderToEquip, itemToDeleteFromInventory));
    }

    /// <summary>
    /// Sets the item to be used.
    /// </summary>
    public void SetItemToUse(Item itemToUse)
    {
        this.itemToUse = itemToUse;
        useButton.interactable = true;
        dropButton.interactable = true;
    }

    /// <summary>
    /// Disables interaction buttons.
    /// </summary>
    public void ButtonOff()
    {
        useButton.interactable = false;
        dropButton.interactable = false;
    }
}
