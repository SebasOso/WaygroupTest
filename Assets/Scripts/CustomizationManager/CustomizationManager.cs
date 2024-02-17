using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizationManager : MonoBehaviour
{
    public static CustomizationManager Instance;
    [SerializeField] int[] clothesArray = new int[5];
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
    public void NextHip()
    {
        HipsManager.Instance.MoveNext();
    }
    public void BackHip()
    {
        HipsManager.Instance.MoveBack();
    }
    public void NextHair()
    {
        HairManager.Instance.MoveNext();
    }
    public void BackHair()
    {
        HairManager.Instance.MoveBack();
    }
    public void NextTorso()
    {
        TorsoManager.Instance.MoveNext();
    }
    public void BackTorso()
    {
        TorsoManager.Instance.MoveBack();
    }
    public void NextLeftLeg()
    {
        LeftLegManager.Instance.MoveNext();
    }
    public void BackLeftLeg()
    {
        LeftLegManager.Instance.MoveBack();
    }
    public void NextRightLeg()
    {
        RightLegManager.Instance.MoveNext();
    }
    public void BackRightLeg()
    {
        RightLegManager.Instance.MoveBack();
    }
}   
