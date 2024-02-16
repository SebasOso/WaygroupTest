using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor", menuName = "Armors/Make New Shoulder", order = 0)]
public class Shoulder : ScriptableObject
{
    [Header("Settings")]
    public int ShoulderIndex;
    public float ShoulderArmor;
    [Header("Inventory")]
    public InventoryItem inventoryItem;

    public float GetArmor()
    {
        return ShoulderArmor;
    }
    public int GetIndex()
    {
        return ShoulderIndex;
    }
    public InventoryItem GetInventoryItem()
    {
        return inventoryItem;
    }
    public void EquipShoulderFromInventory(InventoryItem inventoryItem, Item item)
    {
        ArmorManager.Instance.EquipShoulder(this);
        InventoryManager.Instance.SetNewInventoryShoulder(inventoryItem, item);
    }
}
