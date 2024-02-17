using System;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Waygroup;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    [Header("Inventory Settings")]
    [SerializeField] private InventoryItem[] slots;
    [SerializeField] private GameObject inventoryCanvasGO;
    [SerializeField] private GridLayoutGroup contentInventory;
    [SerializeField] private List<GameObject> itemsGO;

    private PlayerStateMachine playerStateMachine;

    [Header("UI Settings")]
    [SerializeField] private List<GameObject> uiToHide;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip equipClip;
    [SerializeField] private AudioSource audioSource;

    [Header("For Debug")]
    public bool isPaused;

    [Header("Items To Reset")]
    [SerializeField] InventoryItem[] inventoryItemsToReset;

    public event Action inventoryUpdated;
    // Start is called before the first frame update
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        inventoryCanvasGO.SetActive(false);
        slots = new InventoryItem[24];
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }
    public int GetSize()
    {
        return slots.Length;
    }
    void Start()
    {   
        int childCount = contentInventory.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = contentInventory.transform.GetChild(i);
            GameObject gameObject = child.gameObject;
            if (!itemsGO.Contains(gameObject))
            {
                itemsGO.Add(gameObject);
            }
        }
    }
    private void Update() 
    {
        if(InputManager.Instance.InventoryOpenCloseInput)
        {
            if(!isPaused)
            {
                Pause();
            }
            else
            {
                Unpause();
            }
        }    
    }
    public static MenuManager GetPlayerInventory()
    {
        var player = GameObject.FindWithTag("Player");
        return player.GetComponent<MenuManager>();
    }
    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InputManager.Instance.CanRun = false;
        InputManager.Instance.IsRunning = false;
        GetComponent<PlayerController>().Stop();
        isPaused = true;
        playerStateMachine.enabled = false;
        OpenInventory();
    }
    private void Unpause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<PlayerController>().Stop();
        isPaused = false;
        Time.timeScale = 1f;
        playerStateMachine.enabled = true;
        CloseAllMenus();
    }
    private void OpenInventory()
    {
        inventoryCanvasGO.SetActive(true);
        HideAllUI();
    }
    private void CloseAllMenus()
    {
        inventoryCanvasGO.SetActive(false);
        ShowAllUI();
    }
    private void ShowAllUI()
    {
        foreach (var ui in uiToHide)
        {
            ui.SetActive(true);
        }
    }
    private void HideAllUI()
    {
        foreach(var ui in uiToHide)
        {
            ui.SetActive(false);
        }
    }
    private int FindEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
    public InventoryItem GetItemInSlot(int slot)
    {
        return slots[slot];
    }
    private int FindSlot(InventoryItem item)
    {
        return FindEmptySlot();
    }
    public bool HasSpaceFor(InventoryItem item)
    {
        return FindSlot(item) >= 0;
    }
    public bool HasItem(InventoryItem item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (object.ReferenceEquals(slots[i], item))
            {
                return true;
            }
        }
        return false;
    }
    public bool AddToFirstEmptySlot(InventoryItem item)
    {
        if(HasItem(item))
        {
            if(item.GetStackeable() == true)
            {
                item.AddQuantity();
                if (item.GetQuantity() <= 0)
                {
                    item.Reset();
                }
            }
            else
            {
                int i = FindSlot(item);

                if (i < 0)
                {
                    return false;
                }
                slots[i] = item;
                return false;
            }
        }
        else
        {
            int i = FindSlot(item);

            if (i < 0)
            {
                return false;
            }
            slots[i] = item;
            return true;
        }
        return true;
    }
    public void DeleteItemFlomSlot(int index)
    {
        slots[index] = null;
    }
    public void PlayEquip()
    {
        audioSource.clip = equipClip;
        audioSource.Play();
    }
    private void OnApplicationQuit()
    {
        for (int i = 0; i < inventoryItemsToReset.Length; i++)
        {
            inventoryItemsToReset[i].Reset();
        }
    }
}
