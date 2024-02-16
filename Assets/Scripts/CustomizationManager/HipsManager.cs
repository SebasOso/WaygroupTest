using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

public class HipsManager : MonoBehaviour, IEnumerator, IJsonSaveable
{
    public static HipsManager Instance;
    [SerializeField] private GameObject mainAccesories;
    [SerializeField] private List<GameObject> accesoriesList = new List<GameObject>();
    [SerializeField] public int hipsPosition = 0;

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
        accesoriesList[hipsPosition].SetActive(true);
    }
    public bool MoveNext()
    {
        if(hipsPosition >= 0 && hipsPosition < accesoriesList.Count-1)
        {
            hipsPosition ++;
            accesoriesList[hipsPosition].SetActive(true);
            accesoriesList[hipsPosition-1].SetActive(false);
        }
        return hipsPosition < accesoriesList.Count;
    }
    public bool MoveBack()
    {
        if(hipsPosition > 0)
        {
            hipsPosition --;
            accesoriesList[hipsPosition].SetActive(true);
            accesoriesList[hipsPosition+1].SetActive(false);
        }
        return hipsPosition < accesoriesList.Count;
    }
    public void Reset()
    {
        hipsPosition = 0;
        foreach (GameObject accesorie in accesoriesList)
        {
            accesorie.SetActive(false);
        }
        accesoriesList[hipsPosition].SetActive(true);
    }
    public GameObject CurrentAccesorie()
    {
        try
        {
            return accesoriesList[hipsPosition];
        }
        catch (IndexOutOfRangeException)
        {
            throw new InvalidOperationException();
        }
    }

    public JToken CaptureAsJToken()
    {
        if(hipsPosition < 0)
        {
            return JToken.FromObject(0);
        }
        return JToken.FromObject(hipsPosition);
    }

    public void RestoreFromJToken(JToken state)
    {
        int newPosition = state.ToObject<int>();
        hipsPosition = newPosition;
    }
}
