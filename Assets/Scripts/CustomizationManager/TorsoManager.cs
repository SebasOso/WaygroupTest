using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RPG.Saving;
using UnityEngine;

public class TorsoManager : MonoBehaviour, IEnumerator, IJsonSaveable
{
    public static TorsoManager Instance;
    [SerializeField] private GameObject mainAccesories;
    [SerializeField] private List<GameObject> accesoriesList = new List<GameObject>();
    [SerializeField] public int torsoPosition = 0;

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
        accesoriesList[torsoPosition].SetActive(true);
    }
    public bool MoveNext()
    {
        if(torsoPosition >= 0 && torsoPosition < accesoriesList.Count-1)
        {
            torsoPosition ++;
            accesoriesList[torsoPosition].SetActive(true);
            accesoriesList[torsoPosition-1].SetActive(false);
        }
        return torsoPosition < accesoriesList.Count;
    }
    public bool MoveBack()
    {
        if(torsoPosition > 0)
        {
            torsoPosition --;
            accesoriesList[torsoPosition].SetActive(true);
            accesoriesList[torsoPosition+1].SetActive(false);
        }
        return torsoPosition < accesoriesList.Count;
    }
    public void Reset()
    {
        torsoPosition = 0;
        foreach (GameObject accesorie in accesoriesList)
        {
            accesorie.SetActive(false);
        }
        accesoriesList[torsoPosition].SetActive(true);
    }
    public GameObject CurrentAccesorie()
    {
        try
        {
            return accesoriesList[torsoPosition];
        }
        catch (IndexOutOfRangeException)
        {
            throw new InvalidOperationException();
        }
    }
    public JToken CaptureAsJToken()
    {
        if(torsoPosition < 0)
        {
            return JToken.FromObject(0);
        }
        return JToken.FromObject(torsoPosition);
    }

    public void RestoreFromJToken(JToken state)
    {
        int newPosition = state.ToObject<int>();
        torsoPosition = newPosition;
    }
}
