using System;
using UnityEngine;
using Waygroup;

/// <summary>
/// Manages throwing objects in the game.
/// </summary>
public class ThrowManager : MonoBehaviour
{
    public static ThrowManager Instance { get; private set; }

    [HideInInspector] public bool isBeingCarried = false; 
    private Throwable currentThrowable = null; 

    [Header("Settings")]
    [SerializeField] private float baseForce = 100f; 
    [SerializeField] private GameObject uIToThrow;
    [SerializeField] private Transform grabSocket;

    // Tutorial Flags
    private bool firstTime; 
    private bool finishRecord; 
    public event Action OnFirstThrow;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if(TutorialManager.Instance != null)
        {
            TutorialManager.Instance.OnRecord03 += FinishRecord;
        }
        else
        {
            FinishRecord();
        }
    }

    /// <summary>
    /// Grabs the specified object for throwing.
    /// </summary>
    public void Grab(Throwable objectToGrab)
    {
        if (currentThrowable == null && !isBeingCarried)
        {
            Rigidbody rb = objectToGrab.GetComponent<Rigidbody>();
            Collider collider = objectToGrab.GetComponent<Collider>();
            rb.isKinematic = true;
            collider.isTrigger = true;
            objectToGrab.transform.parent = grabSocket;
            objectToGrab.transform.localPosition = Vector3.zero;
            isBeingCarried = true;
            currentThrowable = objectToGrab;
            uIToThrow.SetActive(true);
            InteractionManager.Instance.canInteract = false;
        }
    }

    /// <summary>
    /// Throws the currently carried object.
    /// </summary>
    public void Throw(float holdingTime)
    {
        if (!finishRecord) {
            Debug.Log("NO FINISH RECORD"); return; }
        if (!firstTime)
        {
            Debug.Log("FIRST TIME FALSE");
            OnFirstThrow?.Invoke();
            firstTime = true;
        }
        if (currentThrowable != null && isBeingCarried)
        {
            Rigidbody rb = currentThrowable.GetComponent<Rigidbody>();
            Collider collider = currentThrowable.GetComponent<Collider>();
            collider.isTrigger = false;
            rb.isKinematic = false;
            rb.useGravity = true;
            currentThrowable.OnLoseFocus();
            currentThrowable.transform.parent = null;
            InteractionManager.Instance.canInteract = true;
            InputManager.Instance.IsInteracting = false;
            Debug.Log("THROW" + holdingTime);


            float force = Mathf.Clamp(holdingTime, 0f, 1f);
            float totalForce = force * baseForce;

            Debug.Log("force: " + force);
            Debug.Log("total force: " + totalForce);

            rb.AddForce(Camera.main.transform.forward * totalForce, ForceMode.Impulse);
            currentThrowable.SetDanger();

            isBeingCarried = false;
            currentThrowable = null;
            uIToThrow.SetActive(false);
        }
    }

    /// <summary>
    /// Drops the currently carried object.
    /// </summary>
    public void Drop()
    {
        if (!finishRecord) { return; }
        if (!firstTime) { return; }
        if (currentThrowable != null && isBeingCarried)
        {
            Rigidbody rb = currentThrowable.GetComponent<Rigidbody>();
            Collider collider = currentThrowable.GetComponent<Collider>();
            collider.isTrigger = false;
            rb.isKinematic = false;
            currentThrowable.OnLoseFocus();
            currentThrowable.transform.parent = null;
            InteractionManager.Instance.canInteract = true;
            InputManager.Instance.IsInteracting = false;
            isBeingCarried = false;
            currentThrowable = null;
            uIToThrow.SetActive(false);
        }
    }

    /// <summary>
    /// Marks the recording as finished.
    /// </summary>
    public void FinishRecord()
    {
        finishRecord = true;
    }
}