using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHandler : MonoBehaviour
{
    [SerializeField] private GameObject swordLogic;

    public void EnableSword()
    {
        swordLogic.SetActive(true);
    }
    public void DisableSword()
    {
        swordLogic.SetActive(false);
    }
}
