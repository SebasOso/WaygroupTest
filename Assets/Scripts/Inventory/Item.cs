
using RPG.Inventories;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

/// <summary>
/// Represents an item in the UI of the inventory.
/// </summary>
public class Item : MonoBehaviour
{
    [SerializeField] InventoryItem item;                
    [SerializeField] Image itemSprite;                  
    [SerializeField] Shoulder shoulder;                 
    [SerializeField] HealthPotion healthPotion;         
    [SerializeField] int index;                         
    [SerializeField] Sprite defaultSprite;              
    [SerializeField] TextMeshProUGUI quantityText;     
    [SerializeField] int quantity = 1;                 
    [SerializeField] bool isStackeable = false;        

    // Events
    public event Action OnFirstEquipped;                
    public event Action OnFirstDropped;                 

    private Health player;

    /// <summary>
    /// Called when the object becomes enabled and initializes player reference.
    /// </summary>
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    /// <summary>
    /// Attempts to use the object.
    /// </summary>
    public void UseObject()
    {
        if(TutorialManager.Instance != null)
        {
            if (!TutorialManager.Instance.canEquip) { return; }
        }
        if (shoulder != null)
        {
            if (TutorialManager.Instance != null)
            {
                if (!TutorialManager.Instance.isFirstTimeEquipped)
                {
                    TutorialManager.Instance.isFirstTimeEquipped = true;
                    OnFirstEquipped?.Invoke();
                }
            }
            shoulder.EquipShoulderFromInventory(this.item, this);
        }
        else if (healthPotion != null)
        {
            if (player.isFullHealth) { return; }
            healthPotion.Heal();
            this.item.RestQuantity();
            quantity--;
            quantityText.text = quantity.ToString();
            if (quantity < 1)
            {
                DeleteItemFromInventory();
            }
        }
    }

    /// <summary>
    /// Drops the object.
    /// </summary>
    public void DropObject()
    {
        if (TutorialManager.Instance != null)
        {
            if (!TutorialManager.Instance.canDrop) { return; }
        }
        if (shoulder != null)
        {
            if(TutorialManager.Instance != null)
            {
                if (!TutorialManager.Instance.isFirstTimeDropped)
                {
                    TutorialManager.Instance.isFirstTimeDropped = true;
                    OnFirstDropped?.Invoke();
                }
            }
            Vector3 dropPosition = player.GetComponent<PlayerController>().position;
            GameObject gameObject = Instantiate(item.GetObjectToDrop(), dropPosition, Quaternion.identity);
            DeleteItemFromInventory();
        }
        else if(healthPotion != null)
        {
            Vector3 dropPosition = player.GetComponent<PlayerController>().position;
            GameObject gameObject = Instantiate(item.GetObjectToDrop(), dropPosition, Quaternion.identity);
            this.item.RestQuantity();
            quantity--;
            quantityText.text = quantity.ToString();
            if (quantity < 1)
            {
                DeleteItemFromInventory();
            }
        }
    }
    /// <summary>
    /// Set the item when you click it in the inventory to use it.
    /// </summary>
    public void SetItemToUse()
    {
        if(this.item == null)
        {
            InventoryManager.Instance.ButtonOff();
            return;
        }
        InventoryManager.Instance.SetItemToUse(this);
        if(TutorialManager.Instance != null)
        {
            OnFirstEquipped += TutorialManager.Instance.PlayInventory03;
            OnFirstDropped += TutorialManager.Instance.PlayInventory04;
        }
    }
    /// <summary>
    /// Set the item in the UI socket with the InventoryItem information.
    /// </summary>
    public void SetItem(int index)
    {
        this.item = MenuManager.Instance.GetItemInSlot(index);
        if(item != null)
        {
            quantity = item.GetQuantity();
            quantityText.enabled = item.GetStackeable();
            isStackeable = true;
            quantityText.text = quantity.ToString();
        }
        shoulder = item?.GetShoulder();
        itemSprite.sprite = item?.GetIcon();
        healthPotion = item?.GetHealthPotion();
    }
    /// <summary>
    /// Delete the item from the inventory and from the InventoryItem list.
    /// </summary>
    public void DeleteItemFromInventory()
    {
        this.item = null;
        this.shoulder = null;
        this.healthPotion = null;
        quantityText.enabled = false;
        isStackeable = false;
        quantity = 0;
        this.itemSprite.sprite = defaultSprite;
        MenuManager.Instance.DeleteItemFlomSlot(index);
    }
    /// <summary>
    /// Add quantity to the item if it is stackeable.
    /// </summary>
    public void AddQuantity()
    {
        if (!isStackeable) { return; }
        quantity++;
    }
    /// <summary>
    /// Check if the item has more than one quantity.
    /// </summary>
    public bool HasQuantity()
    {
        if (!isStackeable) { return false; }
        if(quantity > 0) { return true; }
        return false;
    }
}
