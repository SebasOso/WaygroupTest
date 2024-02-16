using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomSellerManager : MonoBehaviour
{
    public static CustomSellerManager Instance;
    [SerializeField] GameObject interactPlayerUI;
    [SerializeField] private bool isPlayerNear = false;
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject firstToSelect;
    [SerializeField] private List<GameObject> gameplayUI;
    public bool isOpen = false;
    GameObject player;
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
    }
    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            interactPlayerUI.SetActive(true);
            isPlayerNear = true;
            other.GetComponent<PlayerStateMachine>().IsNearNPC = true;
            other.GetComponent<InputReader>().IsNPCInteracting = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            interactPlayerUI.SetActive(false);
            isPlayerNear = false;
            other.GetComponent<PlayerStateMachine>().IsNearNPC = false;
            other.GetComponent<InputReader>().IsNPCInteracting = false;
            GetComponent<Animator>().SetBool("isInteracting", false);
        }
    }
    public void OpenShop()
    {
        if(isPlayerNear)
        {
            isOpen = true;
            GetComponent<Animator>().SetBool("isInteracting", true);
            shopUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(firstToSelect);
            player.GetComponent<Armory>().enabled = false;
            player.GetComponent<PlayerStateMachine>().enabled = false;
            HideGameplayUI();
            player.GetComponent<InputReader>().IsShop = true;
            interactPlayerUI.SetActive(false);
        }
    }
    public void CloseShop()
    {
        if(isPlayerNear)
        {
            isOpen = false;
            GetComponent<Animator>().SetBool("isInteracting", false);
            player.GetComponent<PlayerStateMachine>().IsNearNPC = false;
            player.GetComponent<InputReader>().enabled = true;
            player.GetComponent<PlayerStateMachine>().enabled = true;
            player.GetComponent<Armory>().enabled = true;
            ShowGameplayUI();
            player.GetComponent<InputReader>().IsShop = false;
            EventSystem.current.SetSelectedGameObject(null);
            shopUI.SetActive(false);
        }
    }
    private void ShowGameplayUI()
    {
        foreach (GameObject item in gameplayUI)
        {
            item.SetActive(true);
        }
    }
    private void HideGameplayUI()
    {
        foreach (GameObject item in gameplayUI)
        {
            item.SetActive(false);
        }
    }
}
