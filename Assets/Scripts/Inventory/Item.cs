
using RPG.Inventories;
using UnityEngine.UI;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] InventoryItem item;
    [SerializeField] Image itemSprite;
    [SerializeField] string itemId = "Item Id";
    [SerializeField] Shoulder shoulder;
    [SerializeField] HealthPotion healthPotion;
    [SerializeField] int index;
    [SerializeField] Sprite defaultSprite;
    public void UseObject()
    {
        if (shoulder != null)
        {
            shoulder.EquipShoulderFromInventory(this.item, this);
        }
        else if (healthPotion != null)
        {
            healthPotion.Heal();
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
        this.itemSprite.sprite = defaultSprite;
        MenuManager.Instance.DeleteItemFlomSlot(index);
    }
}
