
using System.Collections;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private Item[] itemsInInventory;

    private Item itemToUse;

    [Header("Armor")]
    public InventoryItem shoulderEquipped;

    [Header("Sounds Settings")]
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
    public void UseItem()
    {
        if(itemToUse != null)
        {
            itemToUse.UseObject();
        }
    }
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
    public void SetItemToUse(Item itemToUse)
    {
        this.itemToUse = itemToUse;
        useButton.interactable = true;
        dropButton.interactable = true;
    }
    public void ButtonOff()
    {
        useButton.interactable = false;
        dropButton.interactable = false;
    }
}
