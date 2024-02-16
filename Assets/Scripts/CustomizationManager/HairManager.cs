using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

public class HairManager : MonoBehaviour, IEnumerator, IJsonSaveable
{
    public static HairManager Instance;
    [SerializeField] private GameObject mainAccesories;
    [SerializeField] private List<GameObject> accesoriesList = new List<GameObject>();
    [SerializeField] public int hairPosition = 0;

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
        accesoriesList[hairPosition].SetActive(true);
    }
    public bool MoveNext()
    {
        if(hairPosition >= 0 && hairPosition < accesoriesList.Count-1)
        {
            hairPosition ++;
            accesoriesList[hairPosition].SetActive(true);
            accesoriesList[hairPosition-1].SetActive(false);
        }
        return hairPosition < accesoriesList.Count;
    }
    public bool MoveBack()
    {
        if(hairPosition > 0)
        {
            hairPosition --;
            accesoriesList[hairPosition].SetActive(true);
            accesoriesList[hairPosition+1].SetActive(false);
        }
        return hairPosition < accesoriesList.Count;
    }
    public void Reset()
    {
        hairPosition = 0;
        foreach (GameObject accesorie in accesoriesList)
        {
            accesorie.SetActive(false);
        }
        accesoriesList[hairPosition].SetActive(true);
    }
    public GameObject CurrentAccesorie()
    {
        try
        {
            return accesoriesList[hairPosition];
        }
        catch (IndexOutOfRangeException)
        {
            throw new InvalidOperationException();
        }
    }
    public JToken CaptureAsJToken()
    {
        if(hairPosition < 0)
        {
            return JToken.FromObject(0);
        }
        return JToken.FromObject(hairPosition);
    }

    public void RestoreFromJToken(JToken state)
    {
        int newPosition = state.ToObject<int>();
        hairPosition = newPosition;
    }
}
