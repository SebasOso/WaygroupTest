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
    [SerializeField] Weapon weapon;
    [SerializeField] Shoulder shoulder;
    [SerializeField] int index;
    [SerializeField] Sprite defaultSprite;
    public void UseObject()
    {
        if(weapon != null)
        {
            weapon.EquipWeaponFromInventory(this.item, this);
        }
        if(shoulder != null)
        {
            shoulder.EquipShoulderFromInventory(this.item, this);
        }
    }
    public void SetItem(int index)
    {
        this.item = MenuManager.Instance.GetItemInSlot(index);
        itemId = item?.GetItemID();
        weapon = item.GetWeapon();
        shoulder = item?.GetShoulder();
        itemSprite.sprite = item?.GetIcon();
    }
    public void DeleteItemFromInventory()
    {
        this.item = null;
        this.itemId = null;
        this.weapon = null;
        this.shoulder = null;
        this.itemSprite.sprite = defaultSprite;
        MenuManager.Instance.DeleteItemFlomSlot(index);
    }
}
