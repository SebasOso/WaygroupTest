using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine.UI;
using RPG.Combat;
using UnityEngine;
using RPG.Saving;
using Newtonsoft.Json.Linq;
using System;

public class Item : MonoBehaviour
{
    [SerializeField] InventoryItem item;
    [SerializeField] Image itemSprite;
    [SerializeField] string itemId = "PLATANOOOOOOOOOOOOOOOOOOOOO";
    [SerializeField] Shoulder shoulder;
    [SerializeField] int index;
    [SerializeField] Sprite defaultSprite;
    public void UseObject()
    {
        if(shoulder != null)
        {
            shoulder.EquipShoulderFromInventory(this.item, this);
        }
    }
    public void SetItem(int index)
    {
        this.item = MenuManager.Instance.GetItemInSlot(index);
        itemId = item?.GetItemID();
        shoulder = item?.GetShoulder();
        itemSprite.sprite = item?.GetIcon();
    }
    public void DeleteItemFromInventory()
    {
        this.item = null;
        this.itemId = null;
        this.shoulder = null;
        this.itemSprite.sprite = defaultSprite;
        MenuManager.Instance.DeleteItemFlomSlot(index);
    }
}
