using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightLegManager : MonoBehaviour, IEnumerator
{
    public static RightLegManager Instance;
    [SerializeField] private GameObject mainAccesories;
    [SerializeField] private List<GameObject> accesoriesList = new List<GameObject>();
    [SerializeField] public int rLPosition = 0;

    public object Current => CurrentAccesorie();
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
        accesoriesList[rLPosition].SetActive(true);
    }
    public bool MoveNext()
    {
        if(rLPosition >= 0 && rLPosition < accesoriesList.Count-1)
        {
            rLPosition ++;
            accesoriesList[rLPosition].SetActive(true);
            accesoriesList[rLPosition-1].SetActive(false);
        }
        return rLPosition < accesoriesList.Count;
    }
    public bool MoveBack()
    {
        if(rLPosition > 0)
        {
            rLPosition --;
            accesoriesList[rLPosition].SetActive(true);
            accesoriesList[rLPosition+1].SetActive(false);
        }
        return rLPosition < accesoriesList.Count;
    }
    public void Reset()
    {
        rLPosition = 0;
        foreach (GameObject accesorie in accesoriesList)
        {
            accesorie.SetActive(false);
        }
        accesoriesList[rLPosition].SetActive(true);
    }
    public GameObject CurrentAccesorie()
    {
        try
        {
            return accesoriesList[rLPosition];
        }
        catch (IndexOutOfRangeException)
        {
            throw new InvalidOperationException();
        }
    }
}
