using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftLegManager : MonoBehaviour, IEnumerator
{
    public static LeftLegManager Instance;
    [SerializeField] private GameObject mainAccesories;
    [SerializeField] private List<GameObject> accesoriesList = new List<GameObject>();
    [SerializeField] public int lRPosition = 0;

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
        accesoriesList[lRPosition].SetActive(true);
    }
    public bool MoveNext()
    {
        if(lRPosition >= 0 && lRPosition < accesoriesList.Count-1)
        {
            lRPosition ++;
            accesoriesList[lRPosition].SetActive(true);
            accesoriesList[lRPosition-1].SetActive(false);
        }
        return lRPosition < accesoriesList.Count;
    }
    public bool MoveBack()
    {
        if(lRPosition > 0)
        {
            lRPosition --;
            accesoriesList[lRPosition].SetActive(true);
            accesoriesList[lRPosition+1].SetActive(false);
        }
        return lRPosition < accesoriesList.Count;
    }
    public void Reset()
    {
        lRPosition = 0;
        foreach (GameObject accesorie in accesoriesList)
        {
            accesorie.SetActive(false);
        }
        accesoriesList[lRPosition].SetActive(true);
    }
    public GameObject CurrentAccesorie()
    {
        try
        {
            return accesoriesList[lRPosition];
        }
        catch (IndexOutOfRangeException)
        {
            throw new InvalidOperationException();
        }
    }
}
