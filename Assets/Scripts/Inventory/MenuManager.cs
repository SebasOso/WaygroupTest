using System;
using System.Collections.Generic;
using RPG.Inventories;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Waygroup;

public class MenuManager : MonoBehaviour, IJsonSaveable
{
    public static MenuManager Instance;
    [SerializeField] private InventoryItem[] slots;
    [SerializeField] private GameObject inventoryCanvasGO;
    [SerializeField] private GridLayoutGroup contentInventory;
    [SerializeField] private List<GameObject> itemsGO;
    private PlayerStateMachine playerStateMachine;

    [SerializeField] private List<GameObject> uiToHide;
    public bool isPaused;
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
        InputManager.Instance.CanRun = false;
        InputManager.Instance.IsRunning = false;
        GetComponent<PlayerController>().Stop();
        isPaused = true;
        playerStateMachine.enabled = false;
        OpenInventory();
    }
    private void Unpause()
    {
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
        EventSystem.current.SetSelectedGameObject(itemsGO[0]);
    }
    private void CloseAllMenus()
    {
        inventoryCanvasGO.SetActive(false);
        ShowAllUI();
        EventSystem.current.SetSelectedGameObject(null);
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
        int i = FindSlot(item);

        if (i < 0)
        {
            return false;
        }

        slots[i] = item;
        return true;
    }
    public void DeleteItemFlomSlot(int index)
    {
        slots[index] = null;
    }
    public JToken CaptureAsJToken()
    {
        var slotStrings = new string[24];
        for (int i = 0; i < 24; i++)
        {
            if (slots[i] != null)
            {
                slotStrings[i] = slots[i].GetItemID();
            }
        }
        return JToken.FromObject(slotStrings);
    }

    public void RestoreFromJToken(JToken state)
    {
        var slotStrings = state.ToObject<String[]>();
        for (int i = 0; i < 24; i++)
        {
            slots[i] = InventoryItem.GetFromID(slotStrings[i]);
        }
        if (inventoryUpdated != null)
        {
            inventoryUpdated();
        }
    }
}
