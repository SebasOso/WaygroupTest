using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waygroup;

public class ThrowManager : MonoBehaviour
{
    public static ThrowManager Instance { get; private set; }
    [HideInInspector] public bool isBeingCarried =  false;
    private Throwable currentThrowable = null;

    [Header("Settings")]
    [SerializeField] private float baseForce = 100f;
    [SerializeField] private GameObject uIToThrow;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    public void Grab(Throwable objectToGrab)
    {
        if(currentThrowable == null && isBeingCarried == false)
        {
            objectToGrab.GetComponent<Rigidbody>().isKinematic = true;
            objectToGrab.transform.parent = Camera.main.transform;
            isBeingCarried = true;
            currentThrowable = objectToGrab;
            uIToThrow.SetActive(true);
            InteractionManager.Instance.canInteract = false;
        }
    }
    public void Throw(float holdingTime)
    {
        if(currentThrowable != null && isBeingCarried)
        {
            currentThrowable.GetComponent<Rigidbody>().isKinematic = false;
            currentThrowable.OnLoseFocus();
            currentThrowable.transform.parent = null;
            InteractionManager.Instance.canInteract = true;
            InputManager.Instance.IsInteracting = false;

            float force = Mathf.Clamp(holdingTime, 0f, 1f);
            float totalForce = force * baseForce;


            currentThrowable.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * totalForce, ForceMode.Impulse);

            isBeingCarried = false;
            currentThrowable = null;
            uIToThrow.SetActive(false);
        }
    }
}
