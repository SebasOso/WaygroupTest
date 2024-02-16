using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject Player { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
