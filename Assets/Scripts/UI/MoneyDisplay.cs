using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyDisplay : MonoBehaviour
{
    MoneyManager money;
    private void Awake() 
    {
        money = GameObject.FindWithTag("Player").GetComponent<MoneyManager>();
    }
    private void OnEnable() 
    {
        money = GameObject.FindWithTag("Player").GetComponent<MoneyManager>();
        if(money == null)
        {
            GetComponent<TextMeshProUGUI>().text = "N/A";
            return;
        }
        GetComponent<TextMeshProUGUI>().text = money.moneyAmount.ToString();
    }
}
