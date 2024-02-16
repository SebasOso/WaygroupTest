using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    BaseStats experience;
    private void Awake() 
    {
        experience = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
    }
    void Update()
    {
        if(experience == null)
        {
            GetComponent<TextMeshProUGUI>().text = "N/A";
            return;
        }
        GetComponent<TextMeshProUGUI>().text = experience.GetLevel().ToString();
    }
}
