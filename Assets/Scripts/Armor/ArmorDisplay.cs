using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArmorDisplay : MonoBehaviour
{
    public static ArmorDisplay Instance { get; private set; }
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        SetArmor();
    }
    private void OnEnable()
    {
        SetArmor();
    }
    public void SetArmor()
    {
        GetComponent<TextMeshProUGUI>().text = ArmorManager.Instance.GetArmor().ToString();
    }
}
