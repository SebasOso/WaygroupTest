﻿using System;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(menuName = ("RPG/Inventory/Item"))]
    public class InventoryItem : ScriptableObject, ISerializationCallbackReceiver    
    {
        // CONFIG DATA
        [Tooltip("Auto-generated UUID for saving/loading. Clear this field if you want to generate a new one.")]
        [SerializeField] string itemID = null;

        [Tooltip("The UI icon to represent this item in the inventory.")]
        [SerializeField] Sprite icon = null;
        [SerializeField] Weapon weapon = null;
        [SerializeField] Shoulder shoulder = null;
        [SerializeField] HealthPotion healthPotion = null;
        [SerializeField] private bool isStackeable = false;
        [SerializeField] private int quantity = 1;
        [SerializeField] private GameObject objectToDrop = null;
        // STATE
        static Dictionary<string, InventoryItem> itemLookupCache;
        public Weapon GetWeapon()
        {
            return weapon;
        }
        
        public Sprite GetIcon()
        {
            return icon;
        }

        public string GetItemID()
        {
            return itemID;
        }
        public Shoulder GetShoulder()
        {
            return shoulder;
        }
        public HealthPotion GetHealthPotion()
        {
            return healthPotion;
        }
        public bool GetStackeable()
        {
            return isStackeable;
        }
        public int GetQuantity()
        {
            return quantity;
        }
        public void AddQuantity()
        {
            quantity = quantity + 1;
        }
        public void RestQuantity()
        {
            quantity = quantity - 1;
        }
        public void Reset()
        {
            quantity = 1;
        }
        public GameObject GetObjectToDrop()
        {
            return objectToDrop;
        }
        // PRIVATE

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            // Generate and save a new UUID if this is blank.
            if (string.IsNullOrWhiteSpace(itemID))
            {
                itemID = System.Guid.NewGuid().ToString();
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            // Require by the ISerializationCallbackReceiver but we don't need
            // to do anything with it.
        }
    }
}
