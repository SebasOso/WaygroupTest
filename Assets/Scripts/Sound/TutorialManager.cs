using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages tutorials and instructional audio cues throughout the game.
/// </summary>
public class TutorialManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the TutorialManager.
    /// </summary>
    public static TutorialManager Instance { get; private set; }

    // Objects to activate for tutorials
    [Header("Objects to activate")]
    [SerializeField] private List<GameObject> objectsToGrab;
    [SerializeField] private GameObject objectsToGrabEnemies;
    [SerializeField] private GameObject pickTutorial;
    [SerializeField] private GameObject portalToActivate;

    // Grab Audio Settings
    [Header("Grab Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip record01;
    [SerializeField] private AudioClip record02;
    [SerializeField] private AudioClip record03;
    [SerializeField] private AudioClip record04;

    // Inventory Audio Settings
    [Header("Inventory Audio Settings")]
    [SerializeField] private AudioClip inventoryRecord01;
    [SerializeField] private AudioClip openInventoryRecord;
    [SerializeField] private AudioClip inventoryRecord02;
    [SerializeField] private AudioClip inventoryRecord03;
    [SerializeField] private AudioClip inventoryRecord04;

    // Enemies Audio Settings
    [Header("Enemies Audio Settings")]
    [SerializeField] private AudioClip enemiesRecord01;
    [SerializeField] private AudioClip enemiesRecord02;
    [SerializeField] private AudioClip enemiesRecord03;
    [SerializeField] private AudioClip healRecord;

    // Events
    public event Action OnRecord01;
    public event Action OnRecord03;
    public event Action OnAllPickups;

    // Objects to pick up
    [SerializeField] private List<GameObject> objectsToPick;

    // Tutorial flags
    [HideInInspector] public bool canOpenInventory = false;
    [HideInInspector] public bool canEquip = false;
    [HideInInspector] public bool canDrop = false;
    [HideInInspector] public bool canHeal = false;
    [HideInInspector] public bool isFirstTimeEquipped = false;
    [HideInInspector] public bool isFirstTimeDropped = false;
    [HideInInspector] public bool isFirstHeal = false;

    private bool isAllPickups = false;
    private bool playerAttacked = false;

    private Health player;

    /// <summary>
    /// Initializes the TutorialManager instance and locates the player object.
    /// </summary>
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }
    private void Start()
    {
        Play01();   
        InteractionManager.Instance.OnFirstGrab += Play03;
        OnRecord01 += () => EnableObjects(true);
        ThrowManager.Instance.OnFirstThrow += Play04;
        OnAllPickups += PlayOpenInventory;
        MenuManager.Instance.OnFirstOpen += PlayInventory02;
        player.OnHeal += PlayHeal;
    }

    /// <summary>
    /// Checks if all pickups have been collected.
    /// </summary>
    public void CheckPickUps()
    {
        if (isAllPickups) { return; }
        List<GameObject> activePicks = objectsToPick.Where(pick => pick.activeSelf == true).ToList();
        if (activePicks.Count > 0)
        {
            isAllPickups = false;
            Debug.Log("Objects still active...");
        }
        else
        {
            isAllPickups = true;
            Debug.Log("Inactive all...");
            OnAllPickups?.Invoke();
        }
    }
    private void SetAudioAndPlay(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }

    /// <summary>
    /// Sets the audio clip and plays it after a delay for the first tutorial.
    /// </summary>
    public void Play01()
    {
        StartCoroutine(WaitForAudio01());
    }

    /// <summary>
    /// Methods for the first tutorial of grab objects and throw them.
    /// </summary>
    public void Play02()
    {
        SetAudioAndPlay(record02);
    }
    public void Play03()
    {
        StartCoroutine(WaitForAudio03());
    }
    public void Play04()
    {
        StartCoroutine(WaitForAudio04());
    }

    /// <summary>
    /// Methods for the second tutorial of pick up items and use the inventory.
    /// </summary>
    public void PlayInventory01()
    {
        StartCoroutine(WaitForInventory01());
    }
    public void PlayOpenInventory()
    {
        isAllPickups = true;
        StartCoroutine(WaitForOpenInventory());
    }
    public void PlayInventory02() //Now equip the shoulders
    {
        StartCoroutine(WaitForInventory02());
    }
    public void PlayInventory03() //Now drop the shoulders
    {
        StartCoroutine(WaitForInventory03());
    }
    public void PlayInventory04() //Finished inventory Tutorial
    {
        StartCoroutine(WaitForInventory04());
    }

    /// <summary>
    /// Methods for the third tutorial of grab objects and throw them to the enemies.
    /// </summary>
    public void PlayEnemies01() //Spaw Enemies
    {
        StartCoroutine(WaitForEnemy01());
    }
    public void PlayEnemies02()
    {
        StartCoroutine(WaitForEnemy02());
    }
    public void PlayEnemies03()
    {
        StartCoroutine(WaitForEnemy03());
    }

    /// <summary>
    /// Methods for the fourth tutorial of open the inventory and drink the potions only if you have less of the maximum health.
    /// </summary>
    public void PlayHeal()
    {
        StartCoroutine(WaitForHeal());
    }

    /// <summary>
    /// Couroutines to manage the events and wait for the audios lenght.
    /// </summary>
    private IEnumerator WaitForAudio01()
    {
        SetAudioAndPlay(record01);
        yield return new WaitForSeconds(record01.length);
        OnRecord01?.Invoke();
        Play02();
    }
    private IEnumerator WaitForAudio03()
    {
        SetAudioAndPlay(record03);
        yield return new WaitForSeconds(record03.length);
        OnRecord03?.Invoke();
    }
    private IEnumerator WaitForAudio04()
    {
        SetAudioAndPlay(record04);
        yield return new WaitForSeconds(record04.length + 10);
        PlayInventory01();
    }
    private IEnumerator WaitForInventory01()
    {
        SetAudioAndPlay(inventoryRecord01);
        yield return new WaitForSeconds(inventoryRecord01.length);
        pickTutorial.SetActive(true);
    }
    private IEnumerator WaitForInventory02()
    {
        SetAudioAndPlay(inventoryRecord02);
        yield return new WaitForSeconds(inventoryRecord02.length);
        canEquip = true;
    }
    private IEnumerator WaitForInventory03()
    {
        SetAudioAndPlay(inventoryRecord03);
        yield return new WaitForSeconds(inventoryRecord03.length);
        canDrop = true;
    }
    private IEnumerator WaitForInventory04()
    {
        SetAudioAndPlay(inventoryRecord04);
        yield return new WaitForSeconds(inventoryRecord04.length + 10);
        PlayEnemies01();
    }
    private IEnumerator WaitForOpenInventory()
    {
        SetAudioAndPlay(openInventoryRecord);
        yield return new WaitForSeconds(openInventoryRecord.length);
        canOpenInventory = true;
    }
    private IEnumerator WaitForEnemy01()
    {
        SetAudioAndPlay(enemiesRecord01);
        yield return new WaitForSeconds(enemiesRecord01.length);
        EnemiesTutorial();
    }
    private IEnumerator WaitForEnemy02()
    {
        yield return new WaitForSeconds(1f);
        SetAudioAndPlay(enemiesRecord02);
    }
    private IEnumerator WaitForEnemy03()
    {
        SetAudioAndPlay(enemiesRecord03);
        yield return new WaitForSeconds(enemiesRecord03.length);
        canHeal = true;
    }
    private IEnumerator WaitForHeal()
    {
        SetAudioAndPlay(healRecord);
        yield return new WaitForSeconds(healRecord.length);
        portalToActivate.SetActive(true);
    }
    /// <summary>
    /// Method to manage the first event.
    /// </summary>
    private void EnableObjects(bool enable)
    {
        foreach (var item in objectsToGrab)
        {
            item.SetActive(enable);
        }
    }

    /// <summary>
    /// Method to manage the enemy event.
    /// </summary>
    private void EnemiesTutorial()
    {
        if(!playerAttacked)
        {
            playerAttacked = true;
            EnableObjects(false);
            objectsToGrabEnemies.SetActive(true);
            player.DealDamage(100f);
            SpawnerManager.Instance.Spawn();
            PlayEnemies02();
        }
    }
}
