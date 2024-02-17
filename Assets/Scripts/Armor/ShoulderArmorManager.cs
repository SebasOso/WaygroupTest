using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;
using RPG.Combat;

public class ShoulderArmorManager : MonoBehaviour
{
    public static ShoulderArmorManager Instance;
    [Header("Position")]
    [SerializeField] public Shoulder shoulder;

    [Header("Shoulder Right")]
    [SerializeField] private GameObject mainAccesories;
    [SerializeField] private List<GameObject> accesoriesList = new List<GameObject>();

    [Header("Shoulders Left")]
    [SerializeField] private GameObject mainAccesories02;
    [SerializeField] private List<GameObject> accesoriesList02 = new List<GameObject>();
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
    void Start()
    {
        SetUpLeft();
        SetUpRight();
    }
    private void SetUpRight()
    {
        int childCount = mainAccesories.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = mainAccesories.transform.GetChild(i);
            GameObject gameObject = child.gameObject;
            if (!accesoriesList.Contains(gameObject))
            {
                accesoriesList.Add(gameObject);
            }
        }
        foreach (GameObject accesorie in accesoriesList)
        {
            accesorie.SetActive(false);
        }
        if (shoulder != null)
        {
            accesoriesList[shoulder.GetIndex()].SetActive(true);
        }
        else
        {
            accesoriesList[0].SetActive(true);
        }
    }
    private void SetUpLeft()
    {
        int childCount = mainAccesories02.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = mainAccesories02.transform.GetChild(i);
            GameObject gameObject = child.gameObject;
            if (!accesoriesList02.Contains(gameObject))
            {
                accesoriesList02.Add(gameObject);
            }
        }
        foreach (GameObject accesorie in accesoriesList02)
        {
            accesorie.SetActive(false);
        }
        if (shoulder != null)
        {
            accesoriesList02[shoulder.GetIndex()].SetActive(true);
        }
        else
        {
            accesoriesList02[0].SetActive(true);
        }
    }
    public void SetShoulders(Shoulder shoulderToEquip)
    {
        foreach (GameObject accesorie in accesoriesList)
        {
            accesorie.SetActive(false);
        }
        foreach (GameObject accesorie in accesoriesList02)
        {
            accesorie.SetActive(false);
        }
        shoulder = shoulderToEquip;
        accesoriesList02[shoulderToEquip.GetIndex()].SetActive(true);
        accesoriesList[shoulderToEquip.GetIndex()].SetActive(true);
    }
}
