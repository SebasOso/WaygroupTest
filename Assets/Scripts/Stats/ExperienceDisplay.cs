using System.Collections;
using System.Collections.Generic;
using RPG.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceDisplay : MonoBehaviour
{
    [SerializeField] Experience experience;
    [SerializeField] BaseStats baseStats;
    [SerializeField] private Slider slider;
    private void Start() 
    {
        Debug.Log("level: " + baseStats.GetLevel());
        Debug.Log("Calculatelevel: " + baseStats.CalculateLevel());
        slider.maxValue = baseStats.GetStat(Stat.ExperienceToLevelUp);
        SetValueExp();
        if(baseStats.GetLevel() > 1)
        {
            slider.minValue = baseStats.GetBaseStat(Stat.ExperienceToLevelUp, 1);
        }
    }
    private void OnEnable() 
    {
        experience.OnExperienceGained += SetValueExp;    
        baseStats.OnLevelUP += SetExp;
    }
    private void OnDisable() 
    {
        experience.OnExperienceGained -= SetValueExp;    
        baseStats.OnLevelUP -= SetExp;
    }
    private void SetValueExp()
    {
        slider.value = experience.GetExperience();
    }
    private void SetExp()
    {
        slider.maxValue = baseStats.GetStat(Stat.ExperienceToLevelUp);
        slider.minValue = baseStats.GetBaseStat(Stat.ExperienceToLevelUp, 1);
        slider.value = experience.GetExperience();
    }
}
