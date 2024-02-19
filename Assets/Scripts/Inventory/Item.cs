
using RPG.Inventories;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    [SerializeField] InventoryItem item;
    [SerializeField] Image itemSprite;
    [SerializeField] string itemId = "Item Id";
    [SerializeField] Shoulder shoulder;
    [SerializeField] HealthPotion healthPotion;
    [SerializeField] int index;
    [SerializeField] Sprite defaultSprite;
    [SerializeField] TextMeshProUGUI quantityText;
    [SerializeField] int quantity = 1;
    [SerializeField] bool isStackeable = false;

    private Health player;
    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }
    public void UseObject()
    {
        if (shoulder != null)
        {
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
    public void DropObject()
    {
        if(shoulder != null)
        {
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
    public void SetItemToUse()
    {
        if(this.item == null)
        {
            InventoryManager.Instance.ButtonOff();
            return;
        }
        InventoryManager.Instance.SetItemToUse(this);
    }
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
        itemId = item?.GetItemID();
        shoulder = item?.GetShoulder();
        itemSprite.sprite = item?.GetIcon();
        healthPotion = item?.GetHealthPotion();
    }
    public void DeleteItemFromInventory()
    {
        this.item = null;
        this.itemId = null;
        this.shoulder = null;
        this.healthPotion = null;
        quantityText.enabled = false;
        isStackeable = false;
        quantity = 0;
        this.itemSprite.sprite = defaultSprite;
        MenuManager.Instance.DeleteItemFlomSlot(index);
    }
    public void AddQuantity()
    {
        if (!isStackeable) { return; }
        quantity++;
    }
    public bool HasQuantity()
    {
        if (!isStackeable) { return false; }
        if(quantity > 0) { return true; }
        return false;
    }
}
